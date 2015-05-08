using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnixBoard.Business
{
    public class GameRow
    {
        public int Size { get; set; }
        public IList<GameCell> Cells { get; set; }

        public GameRow(int size)
        {
            Size = size;
            Cells = new List<GameCell>();

            for (int i = 0; i < size; i++)
            {
                Cells.Add(new GameCell());
            }
        }
        public bool IsFull()
        {
            foreach (GameCell cell in Cells)
            {
                if (!cell.IsFull())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
