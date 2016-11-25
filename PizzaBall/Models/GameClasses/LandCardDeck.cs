using System;
using System.Collections.Generic;

namespace PizzaBall.Models.GameClasses
{
    public class LandCardDeck
    {
        public List<LandCard> Deck { get; set; }
        private const int NUM_OF_DUPES = 4;
        private const int SUITS_IN_DECK = 4;
        private const int RESOURCES_IN_DECK = 5;

        public void InitializeDeck()
        {
            Deck = new List<LandCard>();
            var ct = 1;

            for (int k = 1; k <= NUM_OF_DUPES; k++)
            {
                for (int i = 1; i <= SUITS_IN_DECK; i++)
                {
                    for (int j = 1; j <= RESOURCES_IN_DECK; j++)
                    {
                        Deck.Add(new LandCard
                        {
                            CardId = ct,
                            CardNumber = i,
                            CardResource = j
                        });

                        ct++;
                    }
                }
            }
        }

        public void DrawPuzzleCard(Player p)
        {
            var drawnCard = new LandCard();
            Random r = new Random();
            int rInt = r.Next(1, Deck.Count);

            drawnCard = Deck[rInt];
            Deck.RemoveAt(rInt);

            //Starting hands don't seem very random without this
            System.Threading.Thread.Sleep(10);

            p.Hand.Add(drawnCard);
        }
    }
}