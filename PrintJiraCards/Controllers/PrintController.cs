using PrintJiraCards.Models;
using PrintJiraCards.Services;
using System.Web.Mvc;

namespace PrintJiraCards.Controllers
{
    public class PrintController : Controller
    {
        // GET: Print
        public ActionResult Index()
        {
            var prevJql = "Sprint=73 ORDER BY Rank ";

            if (Session["prevJql"] != null)
                prevJql = (string)Session["prevJql"];

            return View(new PrevJql { Jql = prevJql });
        }

        [HttpPost]
        public ActionResult GenerateCard(string jql)
        {
            //var cards = @"C:\Work\PrintJiraCards\PrintJiraCards\bin\cards.json".ReadFrom<List<Card>>();

            var pg = new PrintGenerator();

            Session["prevJql"] = jql;

            var tasks = pg.GetCards(jql);
            var cards = pg.GenerateCards(tasks);

            return View(cards);
        }
    }
}