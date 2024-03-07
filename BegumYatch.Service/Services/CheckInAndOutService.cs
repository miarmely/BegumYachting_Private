using BegumYatch.Core.DTOs.Error;
using BegumYatch.Core.Models.AdminPanel;
using BegumYatch.Core.QueryParameters;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.Services;
using BegumYatch.Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Service.Services
{
    public class CheckinAndCheckoutService : ICheckinAndCheckoutService
    {
        private readonly IGenericRepository<Temp> _repository;

        public CheckinAndCheckoutService(IGenericRepository<Temp> repository) =>
            _repository = repository;


        public async Task<PagingList<CheckinAndCheckoutDemandModel>> GetFormsByStatusAsync(
            FormParamsForDisplayFormByStatus formParams,
            HttpContext context)
        {
            #region set sql command
            var totalCount = new SqlParameter("@TotalCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var sql = "EXEC Demand_CheckInAndOut_GetFormsByStatus " +
                "@StatusId = {0}, " +
                "@PageSize = {1}, " +
                "@PageNumber = {2}, " +
                "@TotalCount = {3} OUT";
            #endregion

            #region get forms by status (THROW)
            var demands = await _repository
                .FromSqlRawAsync<CheckinAndCheckoutDemandModel>(
                    sql,
                    formParams.FormStatus,
                    formParams.PageSize,
                    formParams.PageNumber,
                    totalCount);

            if (demands.Count == 0)
                throw new MiarException(
                    404,
                    "NF-D-CaC",
                    "Not Found - Demand - CheckinAndCheckout",
                    "form bulunamadı");
            #endregion

            #region save paging infos to header
            var demandPagingList = await PagingList<CheckinAndCheckoutDemandModel>
                .ToPagingListAsync(
                    demands,
                    (int)totalCount.Value,
                    formParams.PageNumber,
                    formParams.PageSize,
                    "Demand-CheckinAndCheckout",
                    context);
            #endregion

            return demandPagingList;
        }
    }
}
