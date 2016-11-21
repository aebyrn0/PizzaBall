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
            if (ModelState.IsValid == false)
                return View(game);

            return View(game);
        }

        public ActionResult PlayGame(int numOfPlayers = 2)
        {
            game.NumberOfPlayers = numOfPlayers;

            return View(game);
        }

        public ActionResult PlayerAction(int gridXvalue, int gridYvalue)
        {
            var card = new PuzzleCard
            {
                //Description = "Test Played Card",
                //Owner = string.Format("Player {0}", game.CurrentPlayerTurn),
                //Resources = new Dictionary<string, int>()
            };

            game.GameGrid.PlayerTakeSquare(gridXvalue, gridYvalue, card);
            game.IncrementPlayerTurn();

            return View("PlayGame", game);
        }
    }

    public class GameInstance
    {
        [Range(2, 5)]
        public int NumberOfPlayers { get; set; } = 2;
        public int CurrentPlayerTurn = 1;
        public CardGrid GameGrid = new CardGrid();

        public void IncrementPlayerTurn()
        {
            CurrentPlayerTurn++;
            if (CurrentPlayerTurn > NumberOfPlayers)
                CurrentPlayerTurn = 1;
        }
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

        public void PlayerTakeSquare(int x, int y, PuzzleCard card)
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
        public int CardNumber { get; set; }
        public int CardResource { get; set; }
    }

    public class PuzzleCardDeck
    {
        public Dictionary<int, PuzzleCard> Deck { get; set; }
        private const int NUM_OF_DUPES = 4;
        private const int TOTAL_GRID_SIZE = 4;

        public PuzzleCardDeck()
        {
            var ct = 1;

            for (int k = 1; k <= NUM_OF_DUPES; k++)
            {
                for (int i = 1; i <= TOTAL_GRID_SIZE; i++)
                {
                    for (int j = 1; j <= TOTAL_GRID_SIZE; j++)
                    {
                        Deck.Add(ct, new PuzzleCard
                        {
                            CardNumber = i + 1,
                            CardResource = j + 1
                        });

                        ct++;
                    }
                }
            }
        }

        public void ShuffleDeck()
        {

        }
    }
}