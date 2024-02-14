using BegumYatch.Core.DTOs.SecurityServiceDemand;
using BegumYatch.Core.Models.Demands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Services
{
    public interface ISecurityServiceDemandService : IService<SecurityServiceDemand>
    {
        Task AddSecurityServiceDemand(AddSecurityServiceDemandDto addSecurityServiceDemand);
        Task<List<GetAllSecurityServiceDemandsDto>> GetAllSecurityServiceDemands(int userId);
        Task<GetSecurityServiceDemandDto> GetSecurityServiceDemandById(int id, int userId);
    }
}
