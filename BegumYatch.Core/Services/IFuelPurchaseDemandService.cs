using BegumYatch.Core.DTOs.AdminPanel.Demands;
using BegumYatch.Core.DTOs.FuelPurchaseDemand;
using BegumYatch.Core.DTOs.MainPage;
using BegumYatch.Core.Models.Demands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Services
{
    public interface IFuelPurchaseDemandService : IService<FuelPurchaseDemand>
    {
        Task AddFuelPurchaseDemand(AddFuelPurchaseDemandDto addFuelPurchaseDemandDto);
        Task<List<GetAllFuelPurchaseDemandsDto>> GetAllFuelPurchaseDemands(int userId);
        Task<GetFuelPurchaseDemandByIdandUserIdDto> GetFuelPurchaseDemandById(int id, int userId);
        Task<string> AddDemands(CheckIn model);
        Task<List<GetFuelPurchaseDemandByIdandUserIdDto>> GetDemands();
        Task<CheckIn> GetAllInfo(int userId);


        #region By MERT
        Task<List<DemandDtoForFuelPurchase>> GetAllFuelPurchaseDemandsAsync();
        #endregion
    }
}
