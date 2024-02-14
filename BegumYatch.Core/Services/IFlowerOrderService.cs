using BegumYatch.Core.DTOs.FlowerOrder;
using BegumYatch.Core.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Services
{
    public interface IFlowerOrderService : IService<FlowerOrder>
    {
        Task AddFlowerOrder(AddFlowerOrderDto addFlowerOrder);
        Task<List<GetAllFlowerOrdersDto>> GetAllFlowerOrders(int userId);
        Task<GetFlowerOrderByIdDto> GetFlowerOrderById(int id, int userId);
    }
}
