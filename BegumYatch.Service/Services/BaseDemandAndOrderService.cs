using BegumYatch.Core.DTOs.Error;
using BegumYatch.Core.Enums.AdminPanel;
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
    public class BaseDemandAndOrderService : IBaseDemandAndOrderService
    {
        private readonly IGenericRepository<Temp> _repository;

        public BaseDemandAndOrderService(IGenericRepository<Temp> repository)
        {
            _repository = repository;
        }

        public async Task<PagingList<TModel>> GetFormsByStatusAsync<TModel>(
            FormParamsForDisplayFormByStatus formParams,
            string procedureName,
            FormType formType,
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
            var forms = await _repository
                .FromSqlRawAsync<TModel>(
                    sql,
                    procedureName,
                    formParams.FormStatus,
                    formParams.PageSize,
                    formParams.PageNumber,
                    totalCount);

            // when any form not found
            if (forms.Count == 0)
            {
                var firstCharOfFormType = formType.ToString()[0];
                var first2CharOfFormName = formName.Substring(0, 2);
                
                throw new MiarException(
                    404,
                    $"NF-{firstCharOfFormType}-{first2CharOfFormName}",
                    $"Not Found - {formType} - {formName}",
                    "form bulunamadı");
            }
            #endregion

            #region save paging infos to header
            var formPagingList = await PagingList<TModel>
                .ToPagingListAsync(
                    forms,
                    (int)totalCount.Value,
                    formParams.PageNumber,
                    formParams.PageSize,
                    formType + "-" + formName,
                    context);
            #endregion

            return formPagingList;
        }
    }
}
