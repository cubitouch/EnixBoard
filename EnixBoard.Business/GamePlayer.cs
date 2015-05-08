using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnixBoard.Business
{
    public class GamePlayer
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public bool IsAI { get; set; }
        public DateTime LastActivityDate { get; set; }
        public IList<GameCard> Cards { get; set; }

        public GamePlayer()
        {
            Id = Guid.NewGuid();
            Active = false;
            IsAI = false;
            LastActivityDate = DateTime.Now;
            Cards = new List<GameCard>();
        }

        public void Action()
        {
            Active = true;
            LastActivityDate = DateTime.Now;
        }
        public void SwitchToAI(Game game)
        {
            Active = true;
            IsAI = true;
        }
    }
}
