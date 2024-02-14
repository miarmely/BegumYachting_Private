using BegumYatch.Core.DTOs.BerthRezervation;
using BegumYatch.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BegumYatch.API.Controllers
{
    public class BerthRezervationController : Controller
    {
        private readonly IBerthRezervationService _berthRezervationService;

        public BerthRezervationController(IBerthRezervationService berthRezervationService)
        {
            _berthRezervationService = berthRezervationService;
        }


        [HttpPost("AddBerthRezervation")]
        public async Task<IActionResult> AddBerthRezervation([FromBody]AddBerthRezervationDto addBerthRezervationDto)
        {
            await _berthRezervationService.AddBerthRezervation(addBerthRezervationDto);
            return Ok();
        }
    }
}
