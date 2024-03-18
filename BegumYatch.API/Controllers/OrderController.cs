using BegumYatch.API.Filters.AdminPanel.Attributes;
using BegumYatch.Core.DTOs.FlowerOrder;
using BegumYatch.Core.Enums.AdminPanel;
using BegumYatch.Core.Models.AdminPanel.OrderModel;
using BegumYatch.Core.QueryParameters;
using BegumYatch.Core.Services;
using Microsoft.AspNetCore.Mvc;


namespace BegumYatch.API.Controllers
{
    //[Authorize]
    public partial class OrderController : Controller
    {
        private readonly IFlowerOrderService _flowerOrderService;
        private readonly IProvisionOrderService _provisionOrderService;
        private readonly IBaseDemandAndOrderService _baseDemandAndOrderService;

		public OrderController(IFlowerOrderService flowerOrderService, IProvisionOrderService provisionOrderService, IBaseDemandAndOrderService baseDemandAndOrderService, IBaseDemandAndOrderService baseFormService)
		{
			_flowerOrderService = flowerOrderService;
			_provisionOrderService = provisionOrderService;
			_baseDemandAndOrderService = baseDemandAndOrderService;
			_baseFormService = baseFormService;
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

    public partial class OrderController  // By MERT
    {
        private readonly IBaseDemandAndOrderService _baseFormService;

        [HttpGet("adminPanel/order/provision/filter")]
        [MiarApiAuthorize("Admin")]
        public async Task<IActionResult> GetProvisionOrdersByFilter(
            [FromQuery] FormParamsForDisplayFormByStatus formParams)
        {
            var orders = await _baseDemandAndOrderService.GetFormsByStatusAsync
                <ProvisionOrderModel>(
                    formParams,
                    "Order_Provision_GetFormsByStatus",
                    FormCategory.Order,
                    "Provision",
                    HttpContext);

            return Ok(orders);
        }


		[HttpGet("adminPanel/order/provision/answer")]
		[MiarApiAuthorize("Admin")]
		public async Task<IActionResult> AnswerTheProvisionOrder(
			[FromQuery] FormParamsForAnswerTheForm formParams)
		{
			await _baseFormService.AnswerTheFormAsync(
				FormType.ProvisionOrder,
				formParams.FormId,
				formParams.FormStatus,
				HttpContext);

			return NoContent();
		}


		[HttpGet("adminPanel/order/flower/filter")]
        [MiarApiAuthorize("Admin")]
        public async Task<IActionResult> GetFlowerOrdersByFilter(
            [FromQuery] FormParamsForDisplayFormByStatus formParams)
        {
            var orders = await _baseDemandAndOrderService.GetFormsByStatusAsync
                <FlowerOrderModel>(
                    formParams,
                    "Order_Flower_GetFormsByStatus",
                    FormCategory.Order,
                    "Flower",
                    HttpContext);

            return Ok(orders);
        }


		[HttpGet("adminPanel/order/flower/answer")]
		[MiarApiAuthorize("Admin")]
		public async Task<IActionResult> AnswerTheFlowerOrder(
			[FromQuery] FormParamsForAnswerTheForm formParams)
		{
			await _baseFormService.AnswerTheFormAsync(
				FormType.FlowerOrder,
				formParams.FormId,
				formParams.FormStatus,
				HttpContext);

			return NoContent();
		}


		[HttpGet("adminPanel/order/TechnicalAssistanceAndSparePart/filter")]
        [MiarApiAuthorize("Admin")]
        public async Task<IActionResult> GetTechnicalAssistanceAndSparePartOrdersByFilter(
            [FromQuery] FormParamsForDisplayFormByStatus formParams)
        {
            var orders = await _baseDemandAndOrderService.GetFormsByStatusAsync
                <TechnicalAssistanceAndSparePartOrderModel>(
                    formParams,
                    "Order_TechnicalAssistanceAndSparePart_GetFormsByStatus",
                    FormCategory.Order,
                    "TechnicalAssistanceAndSparePart",
                    HttpContext);

            return Ok(orders);
        }


		[HttpGet("adminPanel/order/TechnicalAssistanceAndSparePart/answer")]
		[MiarApiAuthorize("Admin")]
		public async Task<IActionResult> AnswerTheTechnicalAssistanceAndSparePartOrder(
			[FromQuery] FormParamsForAnswerTheForm formParams)
		{
			await _baseFormService.AnswerTheFormAsync(
				FormType.TechnicalAssistanceAndSparePartOrder,
				formParams.FormId,
				formParams.FormStatus,
				HttpContext);

			return NoContent();
		}
	}
}