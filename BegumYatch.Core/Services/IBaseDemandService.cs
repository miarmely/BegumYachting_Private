using BegumYatch.Core.QueryParameters;
using BegumYatch.Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;


namespace BegumYatch.Core.Services
{
    public interface IBaseDemandService
    {
        Task<PagingList<TModel>> GetFormsByStatusAsync<TModel>(
            FormParamsForDisplayFormByStatus formParams,
            string procedureName,
            string formName,  // for header and exception
            HttpContext context)
            where TModel : class;
    }
}
