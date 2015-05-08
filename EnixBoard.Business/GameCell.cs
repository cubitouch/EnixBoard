using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnixBoard.Business
{
    public class GameCell
    {
        public GameCard Card { get; set; }

        public bool IsFull()
        {
            return (Card != null);
        }
    }
}
