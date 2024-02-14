using BegumYatch.Core.DTOs.ExcursionDemand;
using BegumYatch.Core.Models.Demands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Services
{
    public interface IExcursionDemandService : IService<ExcursionDemand>
    {
        Task AddExcursionDemand(AddExcursionDemandDto addExcursionDemand);
        Task<List<GetAllExcursionDemandDto>> GetAllExcursionDemandServices(int userId);
        Task<GetExcursionDemandByIdandUserIdDto> GetExcursionDemandById(int id, int userId);
    }
}
