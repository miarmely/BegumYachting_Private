using BegumYatch.API.Filters.AdminPanel.Attributes;
using Microsoft.AspNetCore.Mvc;


namespace BegumYacht_Web.Controllers
{
    [MiarWebAuthorize("Admin")]
    public partial class MenubarController : Controller // account
    {
        public IActionResult Profile() => View();
    }
}
