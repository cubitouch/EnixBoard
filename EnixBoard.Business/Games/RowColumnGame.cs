using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnixBoard.Business.Games
{
    public partial class RowColumnGame : Game
    {
        public override string GameTitle
        {
            get
            {
                return "Row & Column";
            }
        }
        public WinningSideEnum WinningSide { get; set; }
        public int WinningIndex { get; set; }
        public override string WinningRule()
        {
            return "You play either in lines or in columns, according to the top or left indicators. The player who will have the best score on its orientation win.";
        }
        protected override void GenerateDesk()
        {
            Random random = new Random();

            for (int i = 0; i < 25; i++)
            {
                Deck.Add(new GameCard(random.Next(1, 6)));
            }
        }
        protected override void InitBoard()
        {
            Board.Size = 5;
            for (int i = 0; i < 5; i++)
            {
                Board.Rows.Add(new GameRow(5));
            }
        }
        public override void End()
        {
            List<int> resultsRow = new List<int>();
            List<int> resultsColumn = new List<int>();
            resultsColumn.AddRange(new List<int> { 0, 0, 0, 0, 0 });

            foreach (GameRow row in Board.Rows)
            {
                int sumA = 0;
                foreach (GameCell cell in row.Cells)
                {
                    sumA += cell.Card.Value;
                    resultsColumn[row.Cells.IndexOf(cell)] += cell.Card.Value;
                }
                resultsRow.Add(sumA);
            }

            // winner A
            if (resultsRow.Max() < resultsColumn.Max())
            {
                WinnerId = PlayerA.Id;
                WinningSide = WinningSideEnum.Columns;
                WinningIndex = resultsColumn.IndexOf(resultsColumn.Max());
            }
            // winner A
            else if (resultsRow.Max() > resultsColumn.Max())
            {
                WinnerId = PlayerB.Id;
                WinningSide = WinningSideEnum.Rows;
                WinningIndex = resultsRow.IndexOf(resultsRow.Max());
            }
            // no winner
            else
            {
                WinnerId = new Guid();
            }
        }
        protected override void PlayAIDelayed(GamePlayer player)
        {
            GameCard card = player.Cards.First();
            int x = 0;
            int y = 0;

            Dictionary<int, int> resultsRow = new Dictionary<int, int>();
            Dictionary<int, int> resultsColumn = new Dictionary<int, int>();

            resultsColumn.Add(0, 0);
            resultsColumn.Add(1, 0);
            resultsColumn.Add(2, 0);
            resultsColumn.Add(3, 0);
            resultsColumn.Add(4, 0);

            foreach (GameRow row in Board.Rows)
            {
                int sumA = 0;
                foreach (GameCell cell in row.Cells.Where(c => c.IsFull()))
                {
                    sumA += cell.Card.Value;
                    resultsColumn[row.Cells.IndexOf(cell)] += cell.Card.Value;
                }
                resultsRow.Add(Board.Rows.IndexOf(row), sumA);
            }

            if (card.Value <= 3 && player.Id == PlayerA.Id || card.Value > 3 && player.Id == PlayerB.Id)
            {
                foreach (KeyValuePair<int, int> i in resultsRow.OrderByDescending(i => i.Value))
                {
                    if (!Board.Rows[i.Key].IsFull())
                    {
                        x = i.Key;
                        break;
                    }
                }
                foreach (GameCell cell in Board.Rows[x].Cells)
                {
                    if (!cell.IsFull())
                    {
                        break;
                    }
                    y++;
                }
            }
            else
            {
                foreach (KeyValuePair<int, int> i in resultsColumn.OrderByDescending(i => i.Value))
                {
                    if (!Board.Columns[i.Key].IsFull())
                    {
                        y = i.Key;
                        break;
                    }
                }
                foreach (GameRow row in Board.Rows)
                {
                    if (!row.Cells[y].IsFull())
                    {
                        break;
                    }
                    x++;
                }
            }

            Play(player.Id, card.Id, x, y);
        }
    }
}
