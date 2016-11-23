using System.Collections.Generic;

namespace PizzaBall.Models.GameClasses
{
    public class Player
    {
        public string Name { get; set; }
        public List<LandCard> Hand { get; set; }
    }

}