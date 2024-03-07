using Microsoft.AspNetCore.Mvc;

namespace BegumYacht_Web.Controllers
{
    public class DemandController : Controller
    {
        public IActionResult FuelPurchase()
        {
            return View();
        }

        public IActionResult CheckinAndCheckout()
        {
            return View();
        }
    }
}
