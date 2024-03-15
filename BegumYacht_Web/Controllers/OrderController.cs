using Microsoft.AspNetCore.Mvc;
using BegumYatch.API.Filters.AdminPanel.Attributes;


namespace BegumYacht_Web.Controllers
{
    [MiarWebAuthorize("Admin")]
    public class OrderController : Controller
    {
        public IActionResult Provision() => View();
        public IActionResult Flower() => View();
        public IActionResult TechnicalAssistanceAndSparePart() => View();
    }
}
