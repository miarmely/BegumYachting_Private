using Microsoft.AspNetCore.Mvc;

namespace BegumYacht_Web.Controllers
{
    public class DemandController : Controller
    {
        public IActionResult FuelPurchase() => View();
        public IActionResult CheckinAndCheckout() => View();
        public IActionResult BerthReservation() => View();
        public IActionResult VipTransfer() => View();
    }
}
