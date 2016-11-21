using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PizzaBall.Controllers
{
    public class HomeController : Controller
    {
        public GameInstance game = new GameInstance();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PlayGame(int numOfPlayers=2)
        {
            game.NumberOfPlayers = numOfPlayers;

            return View(game);
        }
    }

    public class GameInstance
    {
        public int NumberOfPlayers = 2;
        public int CurrentPlayerTurn = 1;
        public CardGrid GameGrid = new CardGrid();
   }

    public class CardGrid
    {
        public SortedDictionary<int, List<BoardSlot>> BoardRows {get; set;}
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
                    var newSlot = new BoardSlot{ Yvalue = j };
                    rowSlots.Add(newSlot);
                }
                BoardRows.Add(i, rowSlots);
            }
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
        public string Description {get; set; }
        public string MainImage {get; set; }
        public Dictionary<string, int> Resources { get; set; }
        public string Owner { get; set; }
    }
}