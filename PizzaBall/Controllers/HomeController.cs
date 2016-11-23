using PizzaBall.Models.GameClasses;
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
}