using System.Collections.Generic;
using System.Linq;

namespace PizzaBall.Models.GameClasses
{
    public class Player
    {
        public string Name { get; set; }
        public List<LandCard> Hand { get; set; }
        public List<PointCard> PointCards { get; set; }

        public int Points
        {
            get
            {
                if (PointCards == null || PointCards.Count == 0)
                    return 0;
                else
                    return PointCards.Select(m => m.PointValue).Sum();
            }
        }

        public int Food { get; set; } = 3;
        public int Gold { get; set; }
        public int Stone { get; set; }
        public int Wood { get; set; }
        public int Coal { get; set; }
    }
}