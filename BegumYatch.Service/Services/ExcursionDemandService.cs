using AutoMapper;
using BegumYatch.Core.DTOs.ConciergeServiceDemand;
using BegumYatch.Core.DTOs.ExcursionDemand;
using BegumYatch.Core.DTOs.FuelPurchaseDemand;
using BegumYatch.Core.Enums;
using BegumYatch.Core.Models.Demands;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.Services;
using BegumYatch.Core.UnitOfWorks;
using BegumYatch.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Service.Services
{
    public class ExcursionDemandService : Service<ExcursionDemand>, IExcursionDemandService
    {
        private readonly IGenericRepository<ExcursionDemand> _excursionDemandRepository;
        private readonly IFileOperationService _fileOperationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ExcursionDemandService(IGenericRepository<ExcursionDemand> excursionDemandRepository, IUnitOfWork unitOfWork, IFileOperationService fileOperationService, IMapper mapper) : base(excursionDemandRepository, unitOfWork)
        {
            _excursionDemandRepository = excursionDemandRepository;
            _fileOperationService = fileOperationService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddExcursionDemand([FromBody]AddExcursionDemandDto addExcursionDemand)
        {
            try
            {
                var entity = _mapper.Map<ExcursionDemand>(addExcursionDemand);
                await _excursionDemandRepository.AddAsync(entity);
                await _unitOfWork.CommitAsync();
                //await _fileOperationService.UploadFile(Convert.ToInt16(DemandTypes.ExcursionDemand), entity.Id, addExcursionDemand.Notes);
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public async Task<List<GetAllExcursionDemandDto>> GetAllExcursionDemandServices(int userId)
        {
            var excursionDemands = await _excursionDemandRepository.GetAll()
               .Where(x => x.UserId == userId)
               .OrderByDescending(x => x.Status).ThenByDescending(y => y.CreatedDate)
               .ToListAsync();

            var excursionDemandsDto = _mapper.Map<List<GetAllExcursionDemandDto>>(excursionDemands);
            if (excursionDemandsDto != null && excursionDemandsDto.Count() > 0)
                return excursionDemandsDto;
            else
                return null;
        }

        public async Task<GetExcursionDemandByIdandUserIdDto> GetExcursionDemandById(int id, int userId)
        {
            var excursionDemand = await _excursionDemandRepository.Where(x => x.Id == id && x.UserId == userId).FirstOrDefaultAsync();
            var excursionDemandDto = _mapper.Map<GetExcursionDemandByIdandUserIdDto>(excursionDemand);
            excursionDemandDto.Notes = await _fileOperationService.GetFilesById(Convert.ToInt16(DemandTypes.ExcursionDemand), id);
            if (excursionDemandDto != null)
                return excursionDemandDto;
            else
                return null;
        }
    }
}
