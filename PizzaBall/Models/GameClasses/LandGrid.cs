using System.Collections.Generic;

namespace PizzaBall.Models.GameClasses
{
    public class LandGrid
    {
        public SortedDictionary<int, List<LandSlot>> BoardRows { get; set; }
        private const int BOARD_HEIGHT = 7;
        private const int BOARD_WIDTH = 7;

        public LandGrid()
        {
            BoardRows = new SortedDictionary<int, List<LandSlot>>();

            for (int i = 0; i < BOARD_HEIGHT; i++)
            {
                var rowSlots = new List<LandSlot>();
                for (int j = 0; j < BOARD_WIDTH; j++)
                {
                    var newSlot = new LandSlot { Yvalue = j };
                    rowSlots.Add(newSlot);
                }
                BoardRows.Add(i, rowSlots);
            }
        }

        public void FillSquareWithCard(int x, int y, LandCard card)
        {
            BoardRows[x][y].CardInfo = card;
        }
    }

}