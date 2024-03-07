using BegumYatch.Core.DTOs.ConciergeServiceDemand;
using BegumYatch.Core.DTOs.Error;
using BegumYatch.Core.DTOs.ExcursionDemand;
using BegumYatch.Core.DTOs.FuelPurchaseDemand;
using BegumYatch.Core.DTOs.MainPage;
using BegumYatch.Core.DTOs.ProvisionOrder;
using BegumYatch.Core.DTOs.SecurityServiceDemand;
using BegumYatch.Core.DTOs.TechnicalAssitanceandSparePartOrder;
using BegumYatch.Core.DTOs.VipDemand.BegumYatch.Core.DTOs.VipDemand;
using BegumYatch.Core.Enums;
using BegumYatch.Core.Models.AdminPanel;
using BegumYatch.Core.Models.Demands;
using BegumYatch.Core.QueryParameters;
using BegumYatch.Core.Services;
using BegumYatch.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace BegumYatch.API.Controllers
{
    public partial class DemandController : Controller
    {
        private readonly IFuelPurchaseDemandService _fuelPurchaseDemandService;
        private readonly IConciergeServiceDemandService _conciergeService;
        private readonly IExcursionDemandService _excursionDemandService;
        private readonly ISecurityServiceDemandService _securityServiceDemandService;
        private readonly IFileOperationService _fileOperationService;
        private readonly ITechnicalAssitanceandSparePartOrderService _technicalAssitanceandSparePartOrderService;
        private readonly IProvisionOrderService _provisionOrderService;
        private readonly ICheckinAndCheckoutService _checkinAndCheckoutService;


        public DemandController(IFuelPurchaseDemandService fuelPurchaseDemandService, IConciergeServiceDemandService conciergeService, IExcursionDemandService excursionDemandService, ISecurityServiceDemandService securityServiceDemandService, IFileOperationService fileOperationService, ITechnicalAssitanceandSparePartOrderService technicalAssitanceandSparePartOrderService, IProvisionOrderService provisionOrderService, ICheckinAndCheckoutService checkinAndCheckoutService)
        {
            _fuelPurchaseDemandService = fuelPurchaseDemandService;
            _conciergeService = conciergeService;
            _excursionDemandService = excursionDemandService;
            _securityServiceDemandService = securityServiceDemandService;
            _fileOperationService = fileOperationService;
            _technicalAssitanceandSparePartOrderService = technicalAssitanceandSparePartOrderService;
            _provisionOrderService = provisionOrderService;
            _checkinAndCheckoutService = checkinAndCheckoutService;
        }


        [HttpGet("GetDemands")]
        public async Task<List<GetFuelPurchaseDemandByIdandUserIdDto>> GetDemands()
        {
            var MainPageList = await _fuelPurchaseDemandService.GetDemands();
            //var response = _mapper.Map<List<GetDemandsDto>>(MainPageList);
            return MainPageList;
        }

        [HttpPost("AddDemands")]
        public async Task<String> AddDemands([FromBody]CheckIn model)
        {

            model.DateAdded = DateTime.Now;
            var MainPageList = await _fuelPurchaseDemandService.AddDemands(model);
            
           //await _unitOfWork.CommitAsync();
            //var response = _mapper.Map<String>(MainPageList);
            return MainPageList;
        }

        [HttpGet("GetAllInfo")]
        public async Task<CheckIn> GetAllInfo(int userId)
        {

            
            var MainPageList = await _fuelPurchaseDemandService.GetAllInfo(userId);

            //await _unitOfWork.CommitAsync();
            //var response = _mapper.Map<String>(MainPageList);
            return MainPageList;
        }


        [HttpPost("DownloadFile")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            if (id < 1)
                return BadRequest();

            await _fileOperationService.DownloadFile(id);
            return Ok();
        }

        [HttpPost("AddFuelPurchaseDemand")]
        public async Task<IActionResult> AddFuelPurchaseDemand([FromBody]AddFuelPurchaseDemandDto addFuelPurchaseDemandDto)
        {
            await _fuelPurchaseDemandService.AddFuelPurchaseDemand(addFuelPurchaseDemandDto);
            // await _fileOperationService.UploadFile(addFuelPurchaseDemandDto.Notes);
            return Ok();
        }

        [HttpPost("AddTechnicalAssitance")]
        public async Task<IActionResult> AddTechnicalAssitance([FromBody] AddTechnicalAssitanceDto addTechnicalAssitanceDto)
        {
            await _technicalAssitanceandSparePartOrderService.AddTechnicalAssitanceandSparePartOrder(addTechnicalAssitanceDto);
            return Ok();
        }

        [HttpPost("AddConciergeServiceDemand")]
        public async Task<IActionResult> AddConciergeServiceDemand([FromBody]AddConciergeServiceDemandDto addConciergeServiceDemandDto)
        {
            await _conciergeService.AddConciergeServiceDemand(addConciergeServiceDemandDto);
            return Ok();
        }

        [HttpPost("VipServiceDemand")]
        public async Task<IActionResult> VipServiceDemand([FromBody] AddVipDemand addVipServiceDemandDto)
        {
            await _conciergeService.AddVipServiceDemand(addVipServiceDemandDto);
            return Ok();
        }

        [HttpPost("AddExcursionDemand")]
        public async Task<IActionResult> AddExcursionDemand([FromBody]AddExcursionDemandDto addExcursionDemandDto)
        {
            await _excursionDemandService.AddExcursionDemand(addExcursionDemandDto);
            return Ok();
        }

        [HttpPost("AddSecurityServiceDemand")]
        public async Task<IActionResult> AddSecurityServiceDemand([FromBody]AddSecurityServiceDemandDto addSecurityServiceDemandDto)
        {
            await _securityServiceDemandService.AddSecurityServiceDemand(addSecurityServiceDemandDto);
            return Ok();
        }


        [HttpPost("AddProvisionOrder")]
        public async Task<IActionResult> AddProvisionOrder([FromBody]AddProvisionOrderDto provisionOrderDto)
        {
            await _provisionOrderService.AddProvisionOrder(provisionOrderDto);
            return Ok();
        }

        [HttpGet("GetConciergeServices")]
        public async Task<IActionResult> GetConciergeServices(int userId)
        {
            var concierges = await _conciergeService.GetAllConciergeServices(userId);
            return Ok(concierges);
        }

        [HttpGet("GetConciergeService")]
        public async Task<IActionResult> GetConciergeService(int id, int userId)
        {
            var concierge = await _conciergeService.GetlConciergeServiceById(id, userId);
            return Ok(concierge);
        }

        [HttpGet("GetAllExcursionDemands")]
        public async Task<IActionResult> GetAllExcursionDemands(int userId)
        {
            var excursions = await _excursionDemandService.GetAllExcursionDemandServices(userId);
            return Ok(excursions);
        }

        [HttpGet("GetExcursionDemand")]
        public async Task<IActionResult> GetExcursionDemand(int id, int userId)
        {
            var excursion = await _excursionDemandService.GetExcursionDemandById(id, userId);
            return Ok(excursion);
        }

        [HttpGet("GetAllFuelPurchaseDemands")]
        public async Task<IActionResult> GetAllFuelPurchaseDemands(int userId)
        {
            var fuelPurchases = await _fuelPurchaseDemandService
                .GetAllFuelPurchaseDemands(userId);
            return Ok(fuelPurchases);
        }

        [HttpGet("GetFuelPurchaseDemand")]
        public async Task<IActionResult> GetFuelPurchaseDemand(int id, int userId)
        {
            var fuelPurchase = await _fuelPurchaseDemandService.GetFuelPurchaseDemandById(id, userId);
            return Ok(fuelPurchase);
        }

        [HttpGet("GetAllSecurityServiceDemands")]
        public async Task<IActionResult> GetAllSecurityServiceDemands(int userId)
        {
            var securityDemands = await _securityServiceDemandService.GetAllSecurityServiceDemands(userId);
            return Ok(securityDemands);
        }

        [HttpGet("GetSecurityServiceDemand")]
        public async Task<IActionResult> GetSecurityServiceDemand(int id, int userId)
        {
            var securityDemand = await _securityServiceDemandService.GetSecurityServiceDemandById(id, userId);
            return Ok(securityDemand);
        }

        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders(int userId)
        {
            var concierges = await _conciergeService.GetAllOrdersService(userId);
            return Ok(concierges);
        }


        [HttpGet("GetAllRequests")]
        public async Task<IActionResult> GetAllRequests(int userId)
        {
            var concierges = await _conciergeService.GetAllRequetsService(userId);
            return Ok(concierges);
        }
    }

    public partial class DemandController  // By MERT
    {
        [HttpGet("adminPanel/fuelPurchaseDemand/filter")]
        public async Task<IActionResult> GetFuelPurchaseDemandsByFilter(
            [FromQuery] FormParamsForDisplayFormByStatus formParams)
        {
            var demands = await  _fuelPurchaseDemandService.GetFormsByStatusAsync(
                formParams, 
                HttpContext);

            return Ok(demands);
        }


        [HttpGet("adminPanel/checkinAndCheckout/filter")]
        public async Task<IActionResult> GetCheckinAndCheckoutDemandsByFilter(
            [FromQuery] FormParamsForDisplayFormByStatus formParams)
        {
            var demands = await _checkinAndCheckoutService.GetFormsByStatusAsync(
                formParams,
                HttpContext);

            return Ok(demands);
        }
    }
}