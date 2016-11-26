using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PizzaBall.Models.GameClasses
{
    public class GameInstance
    {
        [Range(2, 5)]
        public int NumberOfPlayers { get; set; } = 2;
        public int CurrentPlayerTurn { get; set; }
        public LandGrid GameGrid { get; set; }
        public Dictionary<int, Player> Players { get; set; }
        public LandCardDeck LandDeck { get; set; }
        public PointCardDeck PointDeck { get; set; }
        public List<PointCard> PointCardsForSale { get; set; }
        public const int STARTING_PLAYER_PUZZLE_CARDS = 7;
        private int STARTING_POINT_CARDS_FOR_SALE = 8;
        public string Message { get; set; }

        public void InitializeGame(int numOfPlayers, string csvFilePath)
        {
            NumberOfPlayers = numOfPlayers;
            CurrentPlayerTurn = 1;

            //Create Game Grid
            GameGrid = new LandGrid();

            //Build land deck
            LandDeck = new LandCardDeck();
            LandDeck.InitializeDeck();

            //Build point card deck
            PointDeck = new PointCardDeck();
            PointDeck.InitializeDeck(csvFilePath);

            //Create correct number of players
            Players = new Dictionary<int, Player>();

            for (int ct = 1; ct <= NumberOfPlayers; ct++)
            {
                //Initialize player
                Players.Add(ct, new Player());

                //Initialize player hand
                Players[ct].Hand = new List<LandCard>();
                Players[ct].PointCards = new List<PointCard>();
            }

            DealPlayerStartingHand();

            PointCardsForSale = new List<PointCard>();
            DealPointCardsForSale();
        }

        private void DealPointCardsForSale()
        {
            for (int i = 1; i <= STARTING_POINT_CARDS_FOR_SALE; i++)
            {
                PointCardsForSale.Add(PointDeck.DealPointCard());
            }
        }

        public void IncrementPlayerTurn()
        {
            CurrentPlayerTurn++;
            if (CurrentPlayerTurn > NumberOfPlayers)
                CurrentPlayerTurn = 1;
        }

        public void DealPlayerStartingHand()
        {
            foreach (Player p in Players.Values)
            {
                for (int ct = 1; ct <= STARTING_PLAYER_PUZZLE_CARDS; ct++)
                {
                    LandDeck.DrawPuzzleCard(p);
                }

                PointDeck.DealScoutCard(p);
            }
        }

        public void PlayerPlayPuzzleCard(int x, int y, int cardId)
        {
            var currentPlayer = Players[CurrentPlayerTurn];
            var cardToPlay = currentPlayer.Hand.Where(c => c.CardId == cardId).FirstOrDefault();

            if (cardToPlay == null)
            {
                throw new Exception(string.Format("Card of cardId: {0} not found.", cardId));
            }

            GameGrid.FillSquareWithCard(x, y, cardToPlay, currentPlayer);
            currentPlayer.Hand.Remove(cardToPlay);
        }

        public void PlayerDrawCard(int playerId)
        {
            LandDeck.DrawPuzzleCard(Players[playerId]);
        }
    }
}