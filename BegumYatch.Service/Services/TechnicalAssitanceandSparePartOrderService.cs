using AutoMapper;
using BegumYatch.Core.DTOs.ProvisionOrder;
using BegumYatch.Core.DTOs.TechnicalAssitanceandSparePartOrder;
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
using static BegumYatch.Core.Permissions.Permissions;

namespace BegumYatch.Service.Services
{
    public class TechnicalAssitanceandSparePartOrderService : Service<TechnicalAssitanceandSparePartOrder>, ITechnicalAssitanceandSparePartOrderService
    {
        private readonly IGenericRepository<TechnicalAssitanceandSparePartOrder> _technicalAssitanceandSparePartOrderRepository;
        private readonly IFileOperationService _fileOperationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TechnicalAssitanceandSparePartOrderService(IGenericRepository<TechnicalAssitanceandSparePartOrder> technicalAssitanceandSparePartOrderRepository, IUnitOfWork unitOfWork, IFileOperationService fileOperationService, IMapper mapper) : base(technicalAssitanceandSparePartOrderRepository, unitOfWork)
        {
            _technicalAssitanceandSparePartOrderRepository = technicalAssitanceandSparePartOrderRepository;
            _fileOperationService = fileOperationService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddTechnicalAssitanceandSparePartOrder(AddTechnicalAssitanceDto addTechnicalAssitanceandSparePartOrder)
        {
            var entity = _mapper.Map<TechnicalAssitanceandSparePartOrder>(addTechnicalAssitanceandSparePartOrder);
            await _technicalAssitanceandSparePartOrderRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
           // await _fileOperationService.UploadFile(Convert.ToInt16(OrderTypes.TechnicalAssitanceOrder), entity.Id, addTechnicalAssitanceandSparePartOrder.Notes);
        }

        public async Task<List<GetAllTechnicalAssitanceandSparePartOrdersDto>> GetAllTechnicalAssitanceandSparePartOrders(int userId)
        {
            var orders = await _technicalAssitanceandSparePartOrderRepository.GetAll()
               .Include(c => c.User)
               .Where(x => x.UserId == userId)
               .OrderByDescending(x => x.Status).ThenByDescending(y => y.CreatedDate)
               .ToListAsync();

            var technicalOrdersDto = _mapper.Map<List<GetAllTechnicalAssitanceandSparePartOrdersDto>>(orders);
            if (technicalOrdersDto != null && technicalOrdersDto.Count() > 0)
                return technicalOrdersDto;
            else
                return null;
        }

        public async Task<GetTechnicalAssitanceandSparePartOrderByIdandUserIdDto> GetTechnicalOrderById(int id, int userId)
        {
            var technicalOrder = await _technicalAssitanceandSparePartOrderRepository.Where(x => x.Id == id && x.UserId == userId).FirstOrDefaultAsync();
            var technicalOrderDto =_mapper.Map<GetTechnicalAssitanceandSparePartOrderByIdandUserIdDto>(technicalOrder);
            technicalOrderDto.Notes = await _fileOperationService.GetFilesById(Convert.ToInt16(OrderTypes.TechnicalAssitanceOrder), id);
            if (technicalOrderDto != null)
                return technicalOrderDto;
            else
                return null;
        }
    }
}
