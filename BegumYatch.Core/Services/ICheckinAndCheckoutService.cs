using BegumYatch.Core.Models.AdminPanel;
using BegumYatch.Core.QueryParameters;
using BegumYatch.Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;


namespace BegumYatch.Core.Services
{
    public interface ICheckinAndCheckoutService
    {
        Task<PagingList<CheckinAndCheckoutDemandModel>> GetFormsByStatusAsync(
            FormParamsForDisplayFormByStatus formParams,
            HttpContext context);
    }
}
