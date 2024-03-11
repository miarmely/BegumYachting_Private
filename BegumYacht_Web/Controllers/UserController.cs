using Microsoft.AspNetCore.Mvc;
using BegumYacht_Web.Filters;


namespace BegumYacht_Web.Controllers
{
    [MiarAuthorize]
    public class UserController : Controller
    {
        public IActionResult Create() => View();
        public IActionResult Display() => View();
    }
}
