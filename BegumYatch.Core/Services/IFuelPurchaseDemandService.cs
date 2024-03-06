using BegumYatch.Core.DTOs.AdminPanel.Demands;
using BegumYatch.Core.DTOs.FuelPurchaseDemand;
using BegumYatch.Core.DTOs.MainPage;
using BegumYatch.Core.Models.AdminPanel;
using BegumYatch.Core.Models.Demands;
using BegumYatch.Core.Models.Demands.AdminPanel;
using BegumYatch.Core.QueryParameters;
using BegumYatch.Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Services
{
    public partial interface IFuelPurchaseDemandService : IService<FuelPurchaseDemand>
    {
        Task AddFuelPurchaseDemand(AddFuelPurchaseDemandDto addFuelPurchaseDemandDto);
        Task<List<GetAllFuelPurchaseDemandsDto>> GetAllFuelPurchaseDemands(int userId);
        Task<GetFuelPurchaseDemandByIdandUserIdDto> GetFuelPurchaseDemandById(int id, int userId);
        Task<string> AddDemands(CheckIn model);
        Task<List<GetFuelPurchaseDemandByIdandUserIdDto>> GetDemands();
        Task<CheckIn> GetAllInfo(int userId);

    }

    public partial interface IFuelPurchaseDemandService
    {
        Task<PagingList<FuelPurchaseDemandModel>> GetFormsByStatusAsync(
            FormParamsForDisplayFormByStatus formParams,
            HttpContext context);
    }
}
