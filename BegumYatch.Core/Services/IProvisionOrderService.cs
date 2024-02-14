using BegumYatch.Core.DTOs.ProvisionOrder;
using BegumYatch.Core.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Services
{
    public interface IProvisionOrderService : IService<ProvisionOrder>
    {
        Task AddProvisionOrder(AddProvisionOrderDto addProvisionOrder);
        Task<List<GetAllProvisionOrdersDto>> GetAllProvisionOrders(int userId);
        Task<GetProvisionOrderByIdDto> GetProvisionOrderById(int id, int userId);
    }
}
