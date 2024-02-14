using BegumYatch.Core.DTOs.TechnicalAssitanceandSparePartOrder;
using BegumYatch.Core.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Services
{
    public interface ITechnicalAssitanceandSparePartOrderService : IService<TechnicalAssitanceandSparePartOrder>
    {
        Task AddTechnicalAssitanceandSparePartOrder(AddTechnicalAssitanceDto addTechnicalAssitanceandSparePartOrder);
        Task<List<GetAllTechnicalAssitanceandSparePartOrdersDto>> GetAllTechnicalAssitanceandSparePartOrders(int userId);
        Task<GetTechnicalAssitanceandSparePartOrderByIdandUserIdDto> GetTechnicalOrderById(int id, int userId);
    }
}
