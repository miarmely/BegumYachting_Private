using Microsoft.AspNetCore.Mvc;
using BegumYatch.API.Filters.AdminPanel.Attributes;


namespace BegumYacht_Web.Controllers
{
    [MiarWebAuthorize("Admin")]
    public class UserController : Controller
    {
        public IActionResult Create() => View();
        public IActionResult Display() => View();
    }
}
