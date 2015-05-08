using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnixBoard.Business
{
    public class GameColumn
    {
        public int Size { get; set; }
        public IList<GameCell> Cells { get; set; }

        public GameColumn(int size)
        {
            Size = size;
            Cells = new List<GameCell>();
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
