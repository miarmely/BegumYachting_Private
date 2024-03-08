using BegumYatch.Core.DTOs.Error;
using BegumYatch.Core.Models.AdminPanel;
using BegumYatch.Core.QueryParameters;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.Services;
using BegumYatch.Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Data;


namespace BegumYatch.Service.Services
{
    public class BaseDemandService : IBaseDemandService
    {
        private readonly IGenericRepository<Temp> _repository;

        public BaseDemandService(IGenericRepository<Temp> repository)
        {
            _repository = repository;
        }

        public async Task<PagingList<TModel>> GetFormsByStatusAsync<TModel>(
            FormParamsForDisplayFormByStatus formParams,
            string procedureName,
            string formName,
            HttpContext context)
            where TModel : class
        {
            #region set sql command
            var totalCount = new SqlParameter("@TotalCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var sql = "EXEC {0} " +
                "@StatusId = {1}, " +
                "@PageSize = {2}, " +
                "@PageNumber = {3}, " +
                "@TotalCount = {4} OUT";
            #endregion

            #region get forms by status (THROW)
            var demands = await _repository
                .FromSqlRawAsync<TModel>(
                    sql,
                    procedureName,
                    formParams.FormStatus,
                    formParams.PageSize,
                    formParams.PageNumber,
                    totalCount);

            if (demands.Count == 0)
                throw new MiarException(
                    404,
                    "NF-D-" + formName.Substring(0, 2),
                    $"Not Found - Demand - {formName}",
                    "form bulunamadı");
            #endregion

            #region save paging infos to header
            var demandPagingList = await PagingList<TModel>
                .ToPagingListAsync(
                    demands,
                    (int)totalCount.Value,
                    formParams.PageNumber,
                    formParams.PageSize,
                    "Demand-" + formName,
                    context);
            #endregion

            return demandPagingList;
        }
    }
}
