using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnixBoard.Business
{
    public abstract class Game
    {
        public virtual string GameTitle
        {
            get
            {
                return this.GetType().Name;
            }
        }
        public Guid Id { get; set; }
        public GameBoard Board { get; set; }

        public Guid CurrentPlayerId { get; set; }
        public GamePlayer PlayerA { get; set; }
        public GamePlayer PlayerB { get; set; }
        public IList<GameCard> Deck { get; set; }

        public Guid WinnerId { get; set; }
        public virtual string WinningRule()
        {
            return "";
        }

        public Game()
        {
            Id = Guid.NewGuid();

            PlayerA = new GamePlayer();
            PlayerB = new GamePlayer();

            Board = new GameBoard();
            Deck = new List<GameCard>();
        }
        public void Init()
        {
            InitBoard();
        }
        public GamePlayer GetPlayerById(Guid id)
        {
            if (PlayerA.Id == id)
            {
                return PlayerA;
            }
            return PlayerB;
        }
        protected void Distribute()
        {
            int i = 0;
            foreach (GameCard card in Deck)
            {
                if (i % 2 == 0)
                {
                    card.PlayerId = PlayerA.Id;
                    PlayerA.Cards.Add(card);
                }
                else
                {
                    card.PlayerId = PlayerB.Id;
                    PlayerB.Cards.Add(card);
                }
                if (i == 5)
                {
                    break;
                }
                i++;
            }

            foreach (GameCard card in PlayerA.Cards)
            {
                Deck.Remove(card);
            }
            foreach (GameCard card in PlayerB.Cards)
            {
                Deck.Remove(card);
            }
        }
        public void Start()
        {
            GenerateDesk();
            Distribute();
            CurrentPlayerId = PlayerA.Id;
            PlayerA.Active = true;
        }
        public void Play(Guid playerId, Guid cardId, int x, int y)
        {
            GamePlayer player = GetPlayerById(playerId);
            GameCell cell = Board.Rows[x].Cells[y];
            cell.Card = player.Cards.First((c) => c.Id == cardId);
            player.Cards.Remove(cell.Card);

            if (Deck.Count > 0)
            {
                GameCard newCard = Deck.First();
                newCard.PlayerId = CurrentPlayerId;
                player.Cards.Add(newCard);
                Deck.Remove(newCard);
            }

            player.Action();
            if (player.Id == PlayerA.Id)
            {
                CurrentPlayerId = PlayerB.Id;
            }
            else
            {
                CurrentPlayerId = PlayerA.Id;
            }

            if (Board.IsFull())
            {
                End();
            }
            else
            {
                player = GetPlayerById(CurrentPlayerId);
                player.Action();
                if (player.IsAI)
                {
                    PlayAI(player);
                }
            }
        }

        public void PlayAI(GamePlayer player)
        {
            Task task = new Task(async () =>
            {
                await Task.Delay(randomPlayAIDelayed.Next(2, 5) * 1000);
                PlayAIDelayed(player);
            });
            task.Start();
        }
        private Random _randomPlayAIDelayed;
        protected Random randomPlayAIDelayed
        {
            get
            {
                if (_randomPlayAIDelayed == null)
                {
                    _randomPlayAIDelayed = new Random();
                }
                return _randomPlayAIDelayed;
            }
        }

        protected virtual void PlayAIDelayed(GamePlayer player) { }
        protected virtual void InitBoard() { }
        protected virtual void GenerateDesk() { }
        public virtual void End() { }
    }
}
