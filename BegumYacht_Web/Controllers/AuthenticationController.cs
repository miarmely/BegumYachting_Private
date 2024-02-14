using Microsoft.AspNetCore.Mvc;

namespace BegumYacht_Web.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
