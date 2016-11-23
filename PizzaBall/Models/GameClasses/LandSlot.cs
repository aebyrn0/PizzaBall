namespace PizzaBall.Models.GameClasses
{
    public class LandSlot
    {
        public int Yvalue { get; set; }
        public LandCard CardInfo { get; set; }

        public bool Occupied()
        {
            return CardInfo != null;
        }
    }

}