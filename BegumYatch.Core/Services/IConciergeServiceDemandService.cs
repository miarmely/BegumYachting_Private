using BegumYatch.Core.DTOs.ConciergeServiceDemand;
using BegumYatch.Core.DTOs.VipDemand.BegumYatch.Core.DTOs.VipDemand;
using BegumYatch.Core.Models.Demands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Services
{
    public interface IConciergeServiceDemandService : IService<ConciergeServiceDemand>
    {
        Task AddConciergeServiceDemand(AddConciergeServiceDemandDto conciergeServiceDemandDto);
        Task AddVipServiceDemand(AddVipDemand vipServiceDemandDto);
        Task<List<GetAllConciergeServiceDto>> GetAllConciergeServices(int userId);
        Task<GetlConciergeServiceByIdandUserIdDto> GetlConciergeServiceById(int id, int userId);
        Task<List<OrderList>> GetAllOrdersService(int userId);
        Task<List<OrderList>> GetAllRequetsService(int userId);
    }
}
