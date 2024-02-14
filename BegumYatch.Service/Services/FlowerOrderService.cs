using AutoMapper;
using BegumYatch.Core.DTOs.ConciergeServiceDemand;
using BegumYatch.Core.DTOs.ExcursionDemand;
using BegumYatch.Core.DTOs.FlowerOrder;
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
    public class FlowerOrderService : Service<FlowerOrder>, IFlowerOrderService
    {
        private readonly IGenericRepository<FlowerOrder> _flowerOrderRepository;
        private readonly IFileOperationService _fileOperationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FlowerOrderService(IGenericRepository<FlowerOrder> flowerOrderRepository, IUnitOfWork unitOfWork, IMapper mapper, IFileOperationService fileOperationService) : base(flowerOrderRepository, unitOfWork)
        {
            _flowerOrderRepository = flowerOrderRepository;
            _fileOperationService = fileOperationService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task AddFlowerOrder(AddFlowerOrderDto addFlowerOrder)
        {
            var entity = _mapper.Map<FlowerOrder>(addFlowerOrder);
            await _flowerOrderRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await _fileOperationService.UploadFile(Convert.ToInt16(OrderTypes.FlowerOrder), entity.Id, addFlowerOrder.note);
        }

        public async Task<List<GetAllFlowerOrdersDto>> GetAllFlowerOrders(int userId)
        {
            var flowerOrders = await _flowerOrderRepository.GetAll()
                .Include(c => c.User)
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Status).ThenBy(y => y.CreatedDate)
                .ToListAsync();

            var flowerOrdersDto = _mapper.Map<List<GetAllFlowerOrdersDto>>(flowerOrders);

            if (flowerOrdersDto != null && flowerOrdersDto.Count() > 0)
                return flowerOrdersDto;
            else
                return null;
        }

        public async Task<GetFlowerOrderByIdDto> GetFlowerOrderById(int id, int userId)
        {
            var flowerOrder = await _flowerOrderRepository.Where(x => x.Id == id && x.UserId == userId).FirstOrDefaultAsync();
            var flowerOrderDto = _mapper.Map<GetFlowerOrderByIdDto>(flowerOrder);
            flowerOrderDto.Notes= await _fileOperationService.GetFilesById(Convert.ToInt16(OrderTypes.FlowerOrder), id);
            if (flowerOrderDto != null)
                return flowerOrderDto;
            else
                return null;
        }
    }
}
