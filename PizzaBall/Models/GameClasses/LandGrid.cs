using System;
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

        public void FillSquareWithCard(int x, int y, LandCard card, Player p)
        {
            CheckIfFirstPlay(x, y);

            SetHighestLowest(x, y);

            BoardRows[x][y].CardInfo = card;
            AwardResourcesForPlayedCard(x, y, card, p);

            SetSquaresNearPlayAsValid(x, y);

            PreventGridLargerThan4x4();
        }

        private void AwardResourcesForPlayedCard(int x, int y, LandCard card, Player p)
        {
            var possiblePoints = 0;


        }

        private void PreventGridLargerThan4x4()
        {
            if ((HighestX - LowestX) == 3)
            {
                if (HighestX + 1 <= BOARD_HEIGHT)
                {
                    for (int i = 0; i < BOARD_WIDTH; i++)
                        BoardRows[HighestX + 1][i].ValidSquare = false;
                }

                if (LowestX - 1 <= 0)
                {
                    for (int i = 0; i < BOARD_WIDTH; i++)
                        BoardRows[LowestX - 1][i].ValidSquare = false;
                }
            }

            if ((HighestY - LowestY) == 3)
            {
                if (HighestY + 1 <= BOARD_WIDTH)
                {
                    for (int i = 0; i < BOARD_HEIGHT; i++)
                        BoardRows[i][HighestY + 1].ValidSquare = false;
                }

                if (LowestY - 1 <= 0)
                {
                    for (int i = 0; i < BOARD_HEIGHT; i++)
                        BoardRows[i][LowestY - 1].ValidSquare = false;
                }
            }
        }

        private void SetSquaresNearPlayAsValid(int x, int y)
        {
            if ((x + 1 < BOARD_HEIGHT) && (x + 1 - LowestX < 4))
                BoardRows[x + 1][y].ValidSquare = true;

            if ((x - 1 >= 0) && (HighestX - (x - 1) < 4))
                BoardRows[x - 1][y].ValidSquare = true;

            if ((y + 1 < BOARD_WIDTH) && (y + 1 - LowestY < 4))
                BoardRows[x][y + 1].ValidSquare = true;

            if ((y - 1 >= 0) && (HighestY - (y - 1) < 4))
                BoardRows[x][y - 1].ValidSquare = true;
        }

        private void SetHighestLowest(int x, int y)
        {
            if (HighestX < x)
                HighestX = x;

            if (HighestY < y)
                HighestY = y;

            if (LowestX > x)
                LowestX = x;

            if (LowestY > y)
                LowestY = y;
        }

        private void CheckIfFirstPlay(int x, int y)
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
        }
    }

}