using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaBall.Models.GameClasses
{
    public class Player
    {
        public string Name { get; set; }
        public List<LandCard> Hand { get; set; }
    }

}