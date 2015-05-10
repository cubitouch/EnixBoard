using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnixBoard.Business;
using EnixBoard.Business.Games;
using EnixBoard.Web.Models;
using System.Reflection;

namespace EnixBoard.Web.Controllers
{
    public class HomeController : Controller
    {
        private static List<SelectListItem> _gameList;
        public static IEnumerable<SelectListItem> GameList
        {
            get
            {
                if (_gameList == null)
                {
                    _gameList = new List<SelectListItem>();
                    foreach (Type type in Assembly.GetAssembly(typeof(Game)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Game))))
                    {
                        SelectListItem item = new SelectListItem();
                        item.Text = (Activator.CreateInstance("EnixBoard.Business", type.FullName).Unwrap() as Game).GameTitle;
                        item.Value = type.FullName;
                        _gameList.Add(item);
                    }
                }
                return _gameList;
            }
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewGame(string gameType)
        {
            Game game = (Game)Activator.CreateInstance("EnixBoard.Business", gameType).Unwrap();
            game.Init();
            game.PlayerA.Action();
            HttpContext.Application[game.Id.ToString()] = game;
            return RedirectToAction("PlayGame", new { id = game.Id, playerId = game.PlayerA.Id });
        }
        public double StartGame(Guid id)
        {
            Game game = (Game)HttpContext.Application[id.ToString()];

            if (new TimeSpan(DateTime.Now.Ticks - game.PlayerB.LastActivityDate.Ticks).TotalSeconds > 15 && !game.PlayerB.Active)
            {
                game.PlayerB.SwitchToAI(game);
                game.Start();
            }
            return Math.Floor(game.PlayerA.Active && game.PlayerB.Active ? 0 : 15 - new TimeSpan(DateTime.Now.Ticks - game.PlayerB.LastActivityDate.Ticks).TotalSeconds + 1);
        }
        public ActionResult JoinGame(JoinModel model)
        {
            Game game = null;
            if (model.Game != null)
            {
                if (HttpContext.Application.AllKeys.Contains(model.Game.Id.ToString()))
                {
                    game = (Game)HttpContext.Application[model.Game.Id.ToString()];
                }
                else
                {
                    foreach (string key in HttpContext.Application.AllKeys)
                    {
                        game = (Game)HttpContext.Application[key];
                        if (game.PlayerB.Active)
                        {
                            game = null;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            if (game != null)
            {
                game.PlayerB.Action();
                game.Start();
                return RedirectToAction("PlayGame", new { id = game.Id, playerId = game.PlayerB.Id });
            }

            if (game == null)
            {
                foreach (string key in HttpContext.Application.AllKeys)
                {
                    game = (Game)HttpContext.Application[key];
                    if (game.ToString() == model.GameType && !game.PlayerB.Active)
                    {
                        game.PlayerB.Action();
                        game.Start();
                        return RedirectToAction("PlayGame", new { id = game.Id, playerId = game.PlayerB.Id });
                    }
                }
            }

            return NewGame(model.GameType);
        }
        public ActionResult PlayGame(Guid id, Guid playerId)
        {
            return View(new JoinModel() { Game = HttpContext.Application[id.ToString()] as Game, PlayerId = playerId });
        }
        public string CurrentPlayerId(Guid id, Guid playerId)
        {
            Game game = (Game)HttpContext.Application[id.ToString()];
            GamePlayer player = game.GetPlayerById(game.CurrentPlayerId);

            if (new TimeSpan(DateTime.Now.Ticks - player.LastActivityDate.Ticks).TotalSeconds > 50 && !player.IsAI)
            {
                player.SwitchToAI(game);
                game.PlayAI(player);
            }
            if (game.Board.IsFull())
            {
                return "";
            }
            if (game.GetPlayerById(playerId).IsAI)
            {
                return "AI";
            }
            return game.CurrentPlayerId.ToString();
        }
        public string WinnerId(Guid id)
        {
            return (HttpContext.Application[id.ToString()] as Game).WinnerId.ToString();
        }
        public ActionResult PlaceCard(Guid id, Guid playerId, Guid cardId, int x, int y)
        {
            Game game = (Game)HttpContext.Application[id.ToString()];
            if (!game.GetPlayerById(playerId).IsAI)
            {
                game.Play(playerId, cardId, x, y);
            }
            return View("Board", new JoinModel() { Game = game, PlayerId = playerId });
        }
        public ActionResult RefreshBoard(Guid id, Guid playerId)
        {
            return View("Board", new JoinModel() { Game = HttpContext.Application[id.ToString()] as Game, PlayerId = playerId });
        }
    }
}