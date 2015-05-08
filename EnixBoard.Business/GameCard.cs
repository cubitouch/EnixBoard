using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnixBoard.Business
{
    public class GameCard
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public int Value { get; set; }

        public GameCard(int value)
        {
            Id = Guid.NewGuid();
            Value = value;
        }
    }
}
