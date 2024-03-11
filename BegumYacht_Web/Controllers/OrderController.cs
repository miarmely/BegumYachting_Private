using Microsoft.AspNetCore.Mvc;
using BegumYacht_Web.Filters;


namespace BegumYacht_Web.Controllers
{
    [MiarAuthorize]
    public class OrderController : Controller
    {
        public IActionResult Provision() => View();
        public IActionResult Flower() => View();
        public IActionResult TechnicalAssistanceAndSparePart() => View();
    }
}
