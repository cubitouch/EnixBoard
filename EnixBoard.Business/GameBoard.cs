using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnixBoard.Business
{
    public class GameBoard
    {
        public int Size { get; set; }
        public IList<GameRow> Rows { get; set; }
        public IList<GameColumn> Columns
        {
            get
            {
                List<GameColumn> columns = new List<GameColumn>();

                for (int i = 0; i < Rows[0].Cells.Count; i++)
                {
                    GameColumn column = new GameColumn(Rows.Count);
                    foreach (GameRow row in Rows)
                    {
                        column.Cells.Add(row.Cells[i]);
                    }
                    columns.Add(column);
                }

                return columns;
            }
        }

        public GameBoard()
        {
            Rows = new List<GameRow>();
        }
        public bool IsFull()
        {
            foreach (GameRow row in Rows)
            {
                if (!row.IsFull())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
