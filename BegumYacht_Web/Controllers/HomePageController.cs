using Microsoft.AspNetCore.Mvc;

namespace BegumYacht_Web.Controllers
{
    public class HomePageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
