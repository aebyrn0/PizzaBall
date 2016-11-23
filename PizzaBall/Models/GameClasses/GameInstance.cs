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
        public LandCardDeck PuzzleDeck { get; set; }
        public const int STARTING_PLAYER_PUZZLE_CARDS = 7;

        public void InitializeGame(int numOfPlayers)
        {
            NumberOfPlayers = numOfPlayers;
            CurrentPlayerTurn = 1;

            //Create Game Grid
            GameGrid = new LandGrid();

            //Build deck
            PuzzleDeck = new LandCardDeck();
            PuzzleDeck.InitializeDeck();

            //Create correct number of players
            Players = new Dictionary<int, Player>();

            for (int ct = 1; ct <= NumberOfPlayers; ct++)
            {
                //Initialize player
                Players.Add(ct, new Player());

                //Initialize player hand
                Players[ct].Hand = new List<LandCard>();
            }

            //Deal player hand
            DealPlayerStartingHand();
        }

        public void IncrementPlayerTurn()
        {
            CurrentPlayerTurn++;
            if (CurrentPlayerTurn > NumberOfPlayers)
                CurrentPlayerTurn = 1;
        }

        public void DealPlayerStartingHand()
        {
            for (int ct = 1; ct <= STARTING_PLAYER_PUZZLE_CARDS; ct++)
            {
                foreach (Player p in Players.Values)
                {
                    p.Hand.Add(PuzzleDeck.DrawPuzzleCard());
                }
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

            GameGrid.FillSquareWithCard(x, y, cardToPlay);
            currentPlayer.Hand.Remove(cardToPlay);
        }

        public void PlayerDrawCard(int playerId)
        {
            Players[playerId].Hand.Add(PuzzleDeck.DrawPuzzleCard());
        }
    }
}