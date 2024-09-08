using ap.Atributos;
using System.Web.Mvc;

namespace ap.Controllers
{
    [AuthSession]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}