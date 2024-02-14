using AutoMapper;
using BegumYatch.Core.DTOs.BerthRezervation;
using BegumYatch.Core.Enums;
using BegumYatch.Core.Models.BerthRezervation;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.Services;
using BegumYatch.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Service.Services
{
    public class BerthRezervationService : Service<BerthRezervation>, IBerthRezervationService
    {
        private readonly IGenericRepository<BerthRezervation> _berthRezervationRepository;
        private readonly IFileOperationService _fileOperationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BerthRezervationService(IGenericRepository<BerthRezervation> berthRezervationRepository, IUnitOfWork unitOfWork, IFileOperationService fileOperationService, IMapper mapper) : base(berthRezervationRepository, unitOfWork)
        {
            _berthRezervationRepository = berthRezervationRepository;
            _fileOperationService = fileOperationService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddBerthRezervation(AddBerthRezervationDto addBerthRezervation)
        {
            var entity = _mapper.Map<BerthRezervation>(addBerthRezervation);
            await _berthRezervationRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            //await _fileOperationService.UploadFile(Convert.ToInt16(RezervationTypes.BertRezervation), entity.Id, addBerthRezervation.Notes);
        }
    }
}
