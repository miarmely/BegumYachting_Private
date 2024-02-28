using Microsoft.AspNetCore.Mvc;

namespace BegumYacht_Web.Controllers
{
    public partial class MenubarController : Controller // account
    {
        public IActionResult Profile()
        {
            return View();
        }
    }

    public partial class MenubarController    // Notifications
    {
    }
}
