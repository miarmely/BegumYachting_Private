using BegumYatch.Core.Enums.AdminPanel;
using BegumYatch.Core.QueryParameters;
using BegumYatch.Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;


namespace BegumYatch.Core.Services
{
    public interface IBaseDemandAndOrderService
    {
        Task<PagingList<TModel>> GetFormsByStatusAsync<TModel>(
            FormParamsForDisplayFormByStatus formParams,
            string procedureName,
            FormType formType,
            string formName,  // for header and exception
            HttpContext context)
            where TModel : class;
    }
}
