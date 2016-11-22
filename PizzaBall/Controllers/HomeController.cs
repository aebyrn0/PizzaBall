using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaBall.Controllers
{
    public class HomeController : Controller
    {
        public GameInstance game = new GameInstance();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session != null && filterContext.HttpContext.Session["gameInstance"] == null)
            {
                filterContext.HttpContext.Session.Add("gameInstance", new GameInstance());
            }

            game = filterContext.HttpContext.Session["gameInstance"] as GameInstance;

            base.OnActionExecuting(filterContext);
        }

        public ActionResult Index()
        {
            return View(game);
        }

        public ActionResult PlayGame(int numOfPlayers = 2)
        {
            game.InitializeGame(numOfPlayers);

            return View(game);
        }

        public ActionResult PlayerAction(int gridXvalue, int gridYvalue, int playCardId)
        {
            game.PlayerPlayPuzzleCard(gridXvalue, gridYvalue, playCardId);
            game.PlayerDrawCard(game.CurrentPlayerTurn);
            game.IncrementPlayerTurn();

            return View("PlayGame", game);
        }
    }

    public class GameInstance
    {
        [Range(2, 5)]
        public int NumberOfPlayers { get; set; } = 2;
        public int CurrentPlayerTurn { get; set; }
        public CardGrid GameGrid { get; set; }
        public Dictionary<int, Player> Players { get; set; }
        public PuzzleCardDeck PuzzleDeck { get; set; }
        public const int STARTING_PLAYER_PUZZLE_CARDS = 7;

        public void InitializeGame(int numOfPlayers)
        {
            NumberOfPlayers = numOfPlayers;
            CurrentPlayerTurn = 1;

            //Create Game Grid
            GameGrid = new CardGrid();

            //Build deck
            PuzzleDeck = new PuzzleCardDeck();
            PuzzleDeck.InitializeDeck();

            //Create correct number of players
            Players = new Dictionary<int, Player>();

            for (int ct = 1; ct <= NumberOfPlayers; ct++)
            {
                //Initialize player
                Players.Add(ct, new Player());

                //Initialize player hand
                Players[ct].Hand = new List<PuzzleCard>();

                //Deal player hand
                DealPlayerStartingHand(Players[ct]);
            }
        }

        public void IncrementPlayerTurn()
        {
            CurrentPlayerTurn++;
            if (CurrentPlayerTurn > NumberOfPlayers)
                CurrentPlayerTurn = 1;
        }

        public void DealPlayerStartingHand(Player p)
        {
            for(int ct = 1; ct <= STARTING_PLAYER_PUZZLE_CARDS; ct++)
            {
                p.Hand.Add(PuzzleDeck.DrawPuzzleCard());
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

    public class Player
    {
        public string Name { get; set; }
        public List<PuzzleCard> Hand { get; set; }
    }

    public class CardGrid
    {
        public SortedDictionary<int, List<BoardSlot>> BoardRows { get; set; }
        private const int BOARD_HEIGHT = 4;
        private const int BOARD_WIDTH = 4;

        public CardGrid()
        {
            BoardRows = new SortedDictionary<int, List<BoardSlot>>();

            for (int i = 0; i < BOARD_HEIGHT; i++)
            {
                var rowSlots = new List<BoardSlot>();
                for (int j = 0; j < BOARD_WIDTH; j++)
                {
                    var newSlot = new BoardSlot { Yvalue = j };
                    rowSlots.Add(newSlot);
                }
                BoardRows.Add(i, rowSlots);
            }
        }

        public void FillSquareWithCard(int x, int y, PuzzleCard card)
        {
            BoardRows[x][y].CardInfo = card;
        }
    }

    public class BoardSlot
    {
        public int Yvalue { get; set; }
        public PuzzleCard CardInfo { get; set; }

        public bool Occupied()
        {
            return CardInfo != null;
        }
    }

    public class PuzzleCard
    {
        public int CardId { get; set; }
        public int CardNumber { get; set; }
        public int CardResource { get; set; }
    }

    public class PuzzleCardDeck
    {
        public List<PuzzleCard> Deck { get; set; }
        private const int NUM_OF_DUPES = 4;
        private const int TOTAL_GRID_SIZE = 4;

        public void InitializeDeck()
        {
            Deck = new List<PuzzleCard>();
            var ct = 1;

            for (int k = 1; k <= NUM_OF_DUPES; k++)
            {
                for (int i = 1; i <= TOTAL_GRID_SIZE; i++)
                {
                    for (int j = 1; j <= TOTAL_GRID_SIZE; j++)
                    {
                        Deck.Add(new PuzzleCard
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

        public PuzzleCard DrawPuzzleCard()
        {
            var drawnCard = new PuzzleCard();
            Random r = new Random();
            int rInt = r.Next(1, Deck.Count);

            drawnCard = Deck[rInt];
            Deck.RemoveAt(rInt);
            System.Threading.Thread.Sleep(100);
            return drawnCard;
        }
    }
}