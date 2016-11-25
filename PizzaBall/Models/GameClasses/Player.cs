using System.Collections.Generic;

namespace PizzaBall.Models.GameClasses
{
    public class Player
    {
        public string Name { get; set; }
        public List<LandCard> Hand { get; set; }
        public Dictionary<string, int> Resources { get; set; }
        public int Food { get; set; } = 3;
        public int Gold { get; set; }
        public int Stone { get; set; }
        public int Wood { get; set; }
        public int Coal { get; set; }
    }
}