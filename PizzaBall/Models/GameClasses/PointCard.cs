using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaBall.Models.GameClasses
{
    public class PointCard
    {
        public int CardId { get; set; }
        public string Name { get; set; }
        public int PointValue { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool IsScoutCard { get; set; }
        public int Gold { get; set; }
        public int Food { get; set; }
        public int Stone { get; set; }
        public int Wood { get; set; }
        public int Coal { get; set; }
        public CardUseFreq Frequency { get; set; }
        public bool PlayerOwned { get; set; }
        public bool Usable { get; set; } = false;
        public bool Used { get; set; } = false;
    }
}