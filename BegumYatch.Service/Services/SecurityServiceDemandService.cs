using AutoMapper;
using BegumYatch.Core.DTOs.FuelPurchaseDemand;
using BegumYatch.Core.DTOs.SecurityServiceDemand;
using BegumYatch.Core.Enums;
using BegumYatch.Core.Models.Demands;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.Services;
using BegumYatch.Core.UnitOfWorks;
using BegumYatch.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Service.Services
{
    public class SecurityServiceDemandService : Service<SecurityServiceDemand>, ISecurityServiceDemandService
    {
        private readonly IGenericRepository<SecurityServiceDemand> _securityServiceDemandRepository;
        private readonly IFileOperationService _fileOperationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SecurityServiceDemandService(IGenericRepository<SecurityServiceDemand> securityServiceDemandRepository, IUnitOfWork unitOfWork, IFileOperationService fileOperationService, IMapper mapper) : base(securityServiceDemandRepository, unitOfWork)
        {
            _securityServiceDemandRepository = securityServiceDemandRepository;
            _fileOperationService = fileOperationService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddSecurityServiceDemand(AddSecurityServiceDemandDto addSecurityServiceDemand)
        {
            var entity = _mapper.Map<SecurityServiceDemand>(addSecurityServiceDemand);
            await _securityServiceDemandRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            //await _fileOperationService.UploadFile(Convert.ToInt16(DemandTypes.SecurityServiceDemand), entity.Id, addSecurityServiceDemand.Notes);
        }

        public async Task<List<GetAllSecurityServiceDemandsDto>> GetAllSecurityServiceDemands(int userId)
        {
            var securityServices = await _securityServiceDemandRepository.GetAll()
               .Include(c => c.User)
               .Where(x => x.UserId == userId)
               .OrderByDescending(x => x.Status).ThenByDescending(y => y.CreatedDate)
               .ToListAsync();

            var securityServicesDto = _mapper.Map<List<GetAllSecurityServiceDemandsDto>>(securityServices);

            if (securityServicesDto != null && securityServicesDto.Count() > 0)
                return securityServicesDto;
            else
                return null;
        }

        public async Task<GetSecurityServiceDemandDto> GetSecurityServiceDemandById(int id, int userId)
        {
            var securityService = await _securityServiceDemandRepository.Where(x => x.Id == id && x.UserId == userId).FirstOrDefaultAsync();
            var securityServiceDto = _mapper.Map<GetSecurityServiceDemandDto>(securityService);
            securityServiceDto.Notes = await _fileOperationService.GetFilesById(Convert.ToInt16(DemandTypes.SecurityServiceDemand), id);
            if (securityServiceDto != null)
                return securityServiceDto;
            else
                return null;
        }
    }
}
