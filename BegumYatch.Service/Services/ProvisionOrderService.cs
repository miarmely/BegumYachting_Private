using AutoMapper;
using AutoMapper.Execution;
using BegumYatch.Core.DTOs.FlowerOrder;
using BegumYatch.Core.DTOs.ProvisionOrder;
using BegumYatch.Core.Enums;
using BegumYatch.Core.Models.Orders;
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
    public class ProvisionOrderService : Service<ProvisionOrder>, IProvisionOrderService
    {
        private readonly IGenericRepository<ProvisionOrder> _provisionOrderRepository;
        private readonly IFileOperationService _fileOperationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProvisionOrderService(IGenericRepository<ProvisionOrder> provisionOrderRepository, IUnitOfWork unitOfWork, IFileOperationService fileOperationService, IMapper mapper) : base(provisionOrderRepository, unitOfWork)
        {
            _provisionOrderRepository = provisionOrderRepository;
            _fileOperationService = fileOperationService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddProvisionOrder(AddProvisionOrderDto addProvisionOrder)
        {

            try
            {
                var entity = _mapper.Map<ProvisionOrder>(addProvisionOrder);
                await _provisionOrderRepository.AddAsync(entity);
                await _unitOfWork.CommitAsync();
                //await _fileOperationService.UploadFile(Convert.ToInt16(OrderTypes.ProvisionOrder), entity.Id, addProvisionOrder.Notes);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public async Task<List<GetAllProvisionOrdersDto>> GetAllProvisionOrders(int userId)
        {
            var provisionOrders = await _provisionOrderRepository
                .GetAll()
                .Include(c => c.User)
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Status)
                .ThenBy(y => y.CreatedDate)
                .ToListAsync();

            var provisionOrdersDto = _mapper.Map<List<GetAllProvisionOrdersDto>>(provisionOrders);

            if (provisionOrdersDto != null && provisionOrdersDto.Count() > 0)
                return provisionOrdersDto;
            else
                return null;
        }

        public async Task<GetProvisionOrderByIdDto> GetProvisionOrderById(int id, int userId)
        {
            var provisionOrder = await _provisionOrderRepository.Where(x => x.Id == id && x.UserId == userId).FirstOrDefaultAsync();
            var provisionOrderDto = _mapper.Map<GetProvisionOrderByIdDto>(provisionOrder);
            provisionOrderDto.Notes= await _fileOperationService.GetFilesById(Convert.ToInt16(OrderTypes.ProvisionOrder), id);
            if (provisionOrderDto != null)
                return provisionOrderDto;
            else
                return null;
        }
    }
}
