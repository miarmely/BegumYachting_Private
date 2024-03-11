using Microsoft.AspNetCore.Mvc;
using BegumYacht_Web.Filters;


namespace BegumYacht_Web.Controllers
{
    [MiarAuthorize]
    public partial class MenubarController : Controller // account
    {
        public IActionResult Profile()
        {
            return View();
        }
    }
}
