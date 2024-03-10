using Microsoft.AspNetCore.Mvc;


namespace BegumYacht_Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Provision() => View();
        public IActionResult Flower() => View();
        public IActionResult TechnicalAssistanceAndSparePart() => View();
    }
}
