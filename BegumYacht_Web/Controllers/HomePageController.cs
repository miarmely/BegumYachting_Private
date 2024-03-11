using BegumYacht_Web.Filters;
using Microsoft.AspNetCore.Mvc;


namespace BegumYacht_Web.Controllers
{
    [MiarAuthorize]
    public class HomePageController : Controller
    {
        public IActionResult Index() => View();
    }
}
