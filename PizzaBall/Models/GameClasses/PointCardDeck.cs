using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
        public List<PointCard> Deck { get; set; }

        public void InitializeDeck(string csvFilePath)
        {
            var cardData = new List<ReadPointDeckCSV>();
            Deck = new List<PointCard>();

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


            foreach(var data in cardData)
            {
                for (int ct=1; ct <= int.Parse(data.Quantity); ct++)
                {
                    CardUseFreq freq = 0;

                    if (data.Action == "clockwise-rotation.png")
                        freq = CardUseFreq.OncePerTurn;
                    if (data.Action == "sunrise.png")
                        freq = CardUseFreq.OncePerRound;
                    else if (data.Action == "back-forth.png")
                        freq = CardUseFreq.Anytime;

                    Deck.Add(new PointCard
                    {
                        Name = data.Name,
                        Coal = int.Parse(data.Coal),
                        Food = int.Parse(data.Food),
                        Gold = int.Parse(data.Gold),
                        Stone = int.Parse(data.Stone),
                        Wood = int.Parse(data.Wood),
                        Description = data.Rule,
                        Image = data.Image,
                        IsScoutCard = (data.Name.Trim().ToLower() == "scout"),
                        Frequency = freq,
                        PointValue = int.Parse(data.Points)
                    });
                }
            }

            var i = Deck;
        }

        public void DealScoutCard(Player p)
        {
            var drawCard = Deck.Where(m => m.IsScoutCard == true).FirstOrDefault();
            Deck.Remove(drawCard);
            p.PointCards.Add(drawCard);
        }

        public PointCard DealNonScoutCard()
        {
            var drawCard = Deck.Where(m => m.IsScoutCard == false).FirstOrDefault();
            Deck.Remove(drawCard);

            return drawCard;
        }
    }

    public enum CardUseFreq
    {
        OncePerTurn,
        OncePerRound,
        Anytime,
    }
}