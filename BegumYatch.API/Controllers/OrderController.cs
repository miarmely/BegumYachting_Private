using BegumYatch.Core.DTOs.FlowerOrder;
using BegumYatch.Core.DTOs.FuelPurchaseDemand;
using BegumYatch.Core.DTOs.ProvisionOrder;
using BegumYatch.Core.DTOs.TechnicalAssitanceandSparePartOrder;
using BegumYatch.Core.Services;
using BegumYatch.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BegumYatch.API.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IFlowerOrderService _flowerOrderService;
        private readonly IProvisionOrderService _provisionOrderService;

        public OrderController(IFlowerOrderService flowerOrderService, IProvisionOrderService provisionOrderService)
        {
            _flowerOrderService = flowerOrderService;
            _provisionOrderService = provisionOrderService;
            
        }

        [HttpPost("AddFlowerOrder")]
        public async Task<IActionResult> AddFlowerOrder(AddFlowerOrderDto addFlowerOrderDto)
        {
            await _flowerOrderService.AddFlowerOrder(addFlowerOrderDto);
            return Ok();
        }

        [HttpGet("GetAllFlowerOrders")]
        public async Task<IActionResult> GetAllFlowerOrders(int userId)
        {
            var flowerOrders = await _flowerOrderService.GetAllFlowerOrders(userId);
            return Ok(flowerOrders);
        }

        [HttpGet("GetFlowerOrder")]
        public async Task<IActionResult> GetFlowerOrder(int id, int userId)
        {
            var flowerOrder = await _flowerOrderService.GetFlowerOrderById(id, userId);
            return Ok(flowerOrder);
        }

        [HttpGet("GetAllProvisionOrders")]
        public async Task<IActionResult> GetAllProvisionOrders(int userId)
        {
            var provisionOrders = await _provisionOrderService.GetAllProvisionOrders(userId);
            return Ok(provisionOrders);
        }
        [HttpGet("GetProvisionOrder")] 
        public async Task<IActionResult> GetProvisionOrder(int id, int userId)
        {
            var provisionOrder = await _provisionOrderService.GetProvisionOrderById(id,userId);
            return Ok(provisionOrder);
        }


        //[HttpGet("GetAllTechnicalOrders")]
        //public async Task<IActionResult> GetAllTechnicalOrders(int userId)
        //{
        //    var technicalOrders = await _technicalAssitanceandSparePartOrderService.GetAllTechnicalAssitanceandSparePartOrders(userId);
        //    return Ok(technicalOrders);
        //}

        //[HttpGet("GetTechnicalOrderByIdAndUserId")]
        //public async Task<IActionResult> GetTechnicalOrderByIdAndUserId(int id, int userId)
        //{
        //    var technicalOrder = await _technicalAssitanceandSparePartOrderService.GetTechnicalOrderById(id, userId);
        //    return Ok(technicalOrder);
        //}
    }
}