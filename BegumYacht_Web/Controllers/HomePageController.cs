using BegumYatch.API.Filters.AdminPanel.Attributes;
using Microsoft.AspNetCore.Mvc;


namespace BegumYacht_Web.Controllers
{
    [MiarWebAuthorize("Admin")]
    public class HomePageController : Controller
    {
        public IActionResult Index() => View();
    }
}
