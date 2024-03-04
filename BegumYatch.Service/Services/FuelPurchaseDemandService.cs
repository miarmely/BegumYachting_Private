using AutoMapper;
using BegumYatch.Core.DTOs.AdminPanel.Demands;
using BegumYatch.Core.DTOs.Error;
using BegumYatch.Core.DTOs.ExcursionDemand;
using BegumYatch.Core.DTOs.FuelPurchaseDemand;
using BegumYatch.Core.DTOs.MainPage;
using BegumYatch.Core.Enums;
using BegumYatch.Core.Models.Demands;
using BegumYatch.Core.Models.Demands.AdminPanel;
using BegumYatch.Core.Models.FileOperations;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.QueryParameters;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.Services;
using BegumYatch.Core.UnitOfWorks;
using BegumYatch.Repository.Repositories;
using BegumYatch.Service.Helper;
using Entities.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Service.Services
{
    public class FuelPurchaseDemandService
        : Service<FuelPurchaseDemand>, IFuelPurchaseDemandService
    {
        private readonly IGenericRepository<FuelPurchaseDemand> _fuelPurchaseDemandRepository;
        private readonly IGenericRepository<CheckIn> _checkInRepository;
        private readonly IGenericRepository<MailOtp> _mailOtpRepository;
        private readonly IFileOperationService _fileOperationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FuelPurchaseDemandService(IGenericRepository<FuelPurchaseDemand> fuelPurchaseDemandRepository, IGenericRepository<CheckIn> checkInRepository, IUnitOfWork unitOfWork, IMapper mapper, IFileOperationService fileOperationService, IGenericRepository<MailOtp> mailOtpRepository) : base(fuelPurchaseDemandRepository, unitOfWork)
        {
            _fuelPurchaseDemandRepository = fuelPurchaseDemandRepository;
            _checkInRepository = checkInRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileOperationService = fileOperationService;
            _mailOtpRepository = mailOtpRepository;

        }

        public async Task<List<GetFuelPurchaseDemandByIdandUserIdDto>> GetDemands()
        {
            var activeDemands = await _fuelPurchaseDemandRepository.GetAll().Where(x => x.Status == true).ToListAsync();
            var passiveDemands = await _fuelPurchaseDemandRepository.GetAll().Where(x => x.Status == false).OrderByDescending(x => x.CreatedDate).ToListAsync();
            var demands = activeDemands.Concat(passiveDemands);
            var mapDemands = _mapper.Map<List<GetFuelPurchaseDemandByIdandUserIdDto>>(demands);
            if (mapDemands != null && mapDemands.Count() > 0)
                return mapDemands;
            else
                throw new Exception("Talep bulunmamaktadır");
        }

        public async Task<String> AddDemands(CheckIn model)
        {


            var entity = _mapper.Map<CheckIn>(model);

            await _checkInRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();

            return "Eklendi";
        }

        public async Task<CheckIn> GetAllInfo(int userId)
        {


            //var entity = _mapper.Map<CheckIn>(model);

            var response = await _checkInRepository.GetAll().Where(x => x.UserId == userId).OrderByDescending(x => x.DateAdded).FirstOrDefaultAsync();

            if (response != null) return response;
            else throw new Exception("Bilgiler Getirilemedi");

        }

        public async Task AddFuelPurchaseDemand(AddFuelPurchaseDemandDto addFuelPurchaseDemandDto)
        {

            var entity = _mapper.Map<FuelPurchaseDemand>(addFuelPurchaseDemandDto);
            await _fuelPurchaseDemandRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            //await _fileOperationService.UploadFile(Convert.ToInt16(DemandTypes.FuelPurchaseDemand), entity.Id, addFuelPurchaseDemandDto.notes);
        }

        public async Task<List<GetAllFuelPurchaseDemandsDto>> GetAllFuelPurchaseDemands(
            int userId)
        {
            var fuelPurchases = await _fuelPurchaseDemandRepository.GetAll()
               .Include(c => c.User)
               .Where(x => x.UserId == userId)
               .OrderByDescending(x => x.Status).ThenByDescending(y => y.CreatedDate)
               .ToListAsync();
            var fuelPurchasesDto = _mapper.Map<List<GetAllFuelPurchaseDemandsDto>>(fuelPurchases);

            if (fuelPurchasesDto != null && fuelPurchasesDto.Count() > 0)
                return fuelPurchasesDto;
            else
                return null;
        }

        public async Task<GetFuelPurchaseDemandByIdandUserIdDto> GetFuelPurchaseDemandById(int id, int userId)
        {
            var fuelPurchase = await _fuelPurchaseDemandRepository.Where(x => x.Id == id && x.UserId == userId).FirstOrDefaultAsync();
            var fuelPurchaseDto = _mapper.Map<GetFuelPurchaseDemandByIdandUserIdDto>(fuelPurchase);
            fuelPurchaseDto.Notes = await _fileOperationService.GetFilesById(Convert.ToInt16(DemandTypes.FuelPurchaseDemand), id);
            if (fuelPurchaseDto != null)
                return fuelPurchaseDto;
            else
                return null;
        }


        #region By MERT
        public async Task<PagingList<DemandDtoForFuelPurchase>> GetAllFuelPurchaseDemandsAsync(
            PagingParams pagingParam,
            HttpContext context)
        {
            #region get all demands
            var entity = await _fuelPurchaseDemandRepository
               .GetAll()
               .Include(d => d.User)
               .OrderByDescending(y => y.CreatedDate)
               .ToListAsync();

            var paginedEntity = entity
                .Skip((pagingParam.PageNumber - 1) * pagingParam.PageSize)
                .Take(pagingParam.PageSize);

            var demands = _mapper
                .Map<List<DemandDtoForFuelPurchase>>(paginedEntity);
            #endregion

            #region when any demand not found  (throw)
            if (demands.Count == 0)
                throw new MiarException(
                    404,
                    "NF-D-FP",
                    "Not Found - Demand - Fuel Purchase",
                    "herhangi bir yakıt alım talebi bulunmamaktadır");
            #endregion

            #region add pagination infos to header
            var demandsInPagingList = await PagingList<DemandDtoForFuelPurchase>
                .ToPagingListAsync(
                    demands,
                    entity.Count,
                    pagingParam.PageNumber,
                    pagingParam.PageSize,
                    "Demand-FuelPurchase",
                    context);
            #endregion

            return demandsInPagingList;
        }

        public async Task<PagingList<AnsweredUnansweredFuelPurchaseDemand>> GetFuelPurchaseDemandsByFilterAsync(
            DemandParamsForAnsweredFuelPurchase demandParams,
            HttpContext context)
        {
            #region set sql command
            var totalCount = new SqlParameter("@TotalCount", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            var sql = "EXEC Demand_FuelPurchase_GetAnsweredDemands " +
                "@StatusId = {0}, " +
                "@PageSize = {1}, " +
                "@PageNumber = {2}, " +
                "@TotalCount = {3} OUT";
            #endregion

            #region get accepted/rejected demands (THROW)
            var demands = await _fuelPurchaseDemandRepository
                .FromSqlRawAsync<AnsweredUnansweredFuelPurchaseDemand>(
                    sql,
                    demandParams.DemandStatus,
                    demandParams.PageSize,
                    demandParams.PageNumber,
                    totalCount);

            if (demands.Count == 0)
                throw new MiarException(
                    404,
                    "NF-D-FP",
                    "Not Found - Demand - Fuel Purchase",
                    "cevaplanmış talep bulunamadı");
            #endregion

            #region save paging infos to header
            var demandPagingList = await PagingList<AnsweredUnansweredFuelPurchaseDemand>
                .ToPagingListAsync(
                    demands,
                    (int)totalCount.Value,
                    demandParams.PageNumber,
                    demandParams.PageSize,
                    "Demand-FuelPurchase",
                    context);
            #endregion

            return demandPagingList;
        }
        #endregion
    }
}