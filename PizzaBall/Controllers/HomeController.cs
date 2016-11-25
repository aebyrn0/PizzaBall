using PizzaBall.Models.GameClasses;
using System.IO;
using System.Reflection;
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
            var filePath = HttpRuntime.AppDomainAppPath + @"Content\GameData\4x4cards.csv";

            game.InitializeGame(numOfPlayers, filePath);

            return View(game);
        }

        public ActionResult PlayerAction(int? gridXvalue, int? gridYvalue, int? playCardId, int? buyCardId, int? useCardId, string action)
        {
            var currentPlayer = game.Players[game.CurrentPlayerTurn];
            game.Message = string.Empty;

            if (action == "playLandCard")
            {
                game.PlayerPlayPuzzleCard(gridXvalue.Value, gridYvalue.Value, playCardId.Value);

                //award resources
            }
            else if (action == "buy")
            {
                var buyCard = game.PointCardsForSale.Find(x => x.CardId == buyCardId.Value);

                if (buyCard != null)
                {
                    if (currentPlayer.Coal >= buyCard.Coal
                        && currentPlayer.Food >= buyCard.Food
                        && currentPlayer.Wood >= buyCard.Wood
                        && currentPlayer.Stone >= buyCard.Stone
                        && currentPlayer.Gold >= buyCard.Gold)
                    {
                        currentPlayer.Coal -= buyCard.Coal;
                        currentPlayer.Food -= buyCard.Food;
                        currentPlayer.Wood -= buyCard.Wood;
                        currentPlayer.Stone -= buyCard.Stone;
                        currentPlayer.Gold -= buyCard.Gold;

                        currentPlayer.PointCards.Add(buyCard);
                        game.PointCardsForSale.Remove(buyCard);
                        game.Message = "Card purchased!";
                    }
                    else
                    {
                        game.Message = "Not enough resources to buy card!";
                    }
                }
            }
            else if (action == "endTurn")
            {
                game.IncrementPlayerTurn();
            }

            return View("PlayGame", game);
        }
    }
}