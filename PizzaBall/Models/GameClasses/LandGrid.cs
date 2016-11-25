using System.Collections.Generic;

namespace PizzaBall.Models.GameClasses
{
    public class LandGrid
    {
        public SortedDictionary<int, List<LandSlot>> BoardRows { get; set; }
        private const int BOARD_HEIGHT = 7;
        private const int BOARD_WIDTH = 7;

        public bool FirstPlay { get; set; } = true;
        public int HighestX { get; set; } = -1;
        public int LowestX { get; set; } = -1;
        public int HighestY { get; set; } = -1;
        public int LowestY { get; set; } = -1;

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
            if (FirstPlay)
            {
                for (int i = 0; i < BOARD_HEIGHT; i++)
                {
                    for (int j = 0; j < BOARD_WIDTH; j++)
                    {
                        BoardRows[i][j].ValidSquare = false;
                    }
                }

                HighestX = LowestX = x;
                HighestY = LowestY = y;

                FirstPlay = false;
            }

            if (HighestX < x)
                HighestX = x;

            if (HighestY < y)
                HighestY = y;

            if (LowestX > x)
                LowestX = x;

            if (LowestY > y)
                LowestY = y;

            BoardRows[x][y].CardInfo = card;

            if ((x+1 < BOARD_HEIGHT) && (x+1 - LowestX < 4))
                BoardRows[x + 1][y].ValidSquare = true;

            if ((x - 1 >= 0) && (HighestX - (x-1) < 4))
                BoardRows[x - 1][y].ValidSquare = true;

            if ((y + 1 < BOARD_HEIGHT) && (y + 1 - LowestY < 4))
                BoardRows[x][y + 1].ValidSquare = true;

            if ((y - 1 >= 0) && (HighestY - (y - 1) < 4))
                BoardRows[x][y - 1].ValidSquare = true;
        }
    }

}