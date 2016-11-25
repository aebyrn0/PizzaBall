using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PizzaBall.Models.GameClasses
{
    public class ReadPointDeckCSV
    {
        public string Name;
        public string Cost_NOT_USED;
        public string Food;
        public string Wood;
        public string Stone;
        public string Coal;
        public string Gold;
        public string Points;
        public string Image;
        public string Quantity;
        public string Action;
        public string Rule;
    }

    public class PointCardDeck
    {
        public List<PointCard> PointCards { get; set; }
        public List<PointCard> ScoutCards { get; set; }

        public void InitializeDeck(string csvFilePath)
        {
            var cardData = new List<ReadPointDeckCSV>();
            PointCards = new List<PointCard>();
            ScoutCards = new List<PointCard>();

            using (TextReader reader = File.OpenText(csvFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var arrLine = line.Split(',');
                    cardData.Add(new ReadPointDeckCSV
                    {
                        Name = arrLine[0],
                        Cost_NOT_USED = arrLine[1],
                        Food = arrLine[2],
                        Wood = arrLine[3],
                        Stone = arrLine[4],
                        Coal = arrLine[5],
                        Gold = arrLine[6],
                        Points = arrLine[7],
                        Image = arrLine[8],
                        Quantity = arrLine[9],
                        Action = arrLine[10],
                        Rule = arrLine[11],
                    });
                }
            }

            foreach (var data in cardData)
            {
                var id = 1;
                for (int ct = 1; ct <= int.Parse(data.Quantity); ct++)
                {
                    if (data.Name.Trim().ToLower() == "scout")
                    {
                        ScoutCards.Add(CreateCardFromData(id, data, true));
                    }
                    else
                    {
                        PointCards.Add(CreateCardFromData(id, data));
                    }

                    id++;
                }
            }
        }

        private PointCard CreateCardFromData(int id, ReadPointDeckCSV data, bool isScout = false)
        {
            CardUseFreq freq = 0;

            if (data.Action == "clockwise-rotation.png")
                freq = CardUseFreq.OncePerTurn;
            if (data.Action == "sunrise.png")
                freq = CardUseFreq.OncePerRound;
            else if (data.Action == "back-forth.png")
                freq = CardUseFreq.Anytime;

            return
                new PointCard
                {
                    CardId = id,
                    Name = data.Name,
                    Coal = int.Parse(data.Coal),
                    Food = int.Parse(data.Food),
                    Gold = int.Parse(data.Gold),
                    Stone = int.Parse(data.Stone),
                    Wood = int.Parse(data.Wood),
                    Description = data.Rule,
                    Image = data.Image,
                    IsScoutCard = isScout,
                    Frequency = freq,
                    PointValue = int.Parse(data.Points),
                    PlayerOwned = false,
                    Usable = (freq == CardUseFreq.Anytime || isScout)
                };
        }

        public void DealScoutCard(Player p)
        {
            //All scout cards are the same so don't need to randomize
            var drawnCard = ScoutCards.FirstOrDefault();

            if (drawnCard != null)
                ScoutCards.Remove(drawnCard);

            drawnCard.PlayerOwned = true;

            p.PointCards.Add(drawnCard);
        }

        public PointCard DealPointCard()
        {
            var drawnCard = new PointCard();
            Random r = new Random();
            int rInt = r.Next(1, PointCards.Count);

            drawnCard = PointCards[rInt];
            PointCards.RemoveAt(rInt);

            //Starting hands don't seem very random without this
            System.Threading.Thread.Sleep(10);

            return drawnCard;
        }
    }

    public enum CardUseFreq
    {
        OncePerTurn,
        OncePerRound,
        Anytime,
    }
}