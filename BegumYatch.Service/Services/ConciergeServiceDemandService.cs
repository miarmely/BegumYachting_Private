using AutoMapper;
using BegumYatch.Core.DTOs.ConciergeServiceDemand;
using BegumYatch.Core.DTOs.TechnicalAssitanceandSparePartOrder;
using BegumYatch.Core.DTOs.VipDemand.BegumYatch.Core.DTOs.VipDemand;
using BegumYatch.Core.Enums;
using BegumYatch.Core.Models.Demands;
using BegumYatch.Core.Models.Orders;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.Services;
using BegumYatch.Core.UnitOfWorks;
using BegumYatch.Repository;
using BegumYatch.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Service.Services
{
    public class ConciergeServiceDemandService : Service<ConciergeServiceDemand>, IConciergeServiceDemandService
    {
        private readonly IGenericRepository<ConciergeServiceDemand> _conciergeServiceDemandRepository;
        private readonly IGenericRepository<VipServiceDemand> _vipServiceDemandRepository;
        private readonly IGenericRepository<AppUser> _userRepository;
        private readonly IFileOperationService _fileOperationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected readonly AppDbContext _context;
        public ConciergeServiceDemandService(AppDbContext context,IGenericRepository<ConciergeServiceDemand> conciergeServiceDemandRepository, IGenericRepository<VipServiceDemand> vipServiceDemandRepository, IUnitOfWork unitOfWork, IFileOperationService fileOperationService, IMapper mapper, IGenericRepository<AppUser> userRepository) : base(conciergeServiceDemandRepository, unitOfWork)
        {
            _conciergeServiceDemandRepository = conciergeServiceDemandRepository;
            _vipServiceDemandRepository = vipServiceDemandRepository;
            _fileOperationService = fileOperationService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        public async Task AddConciergeServiceDemand(AddConciergeServiceDemandDto conciergeServiceDemandDto)
        {
            var entity = _mapper.Map<ConciergeServiceDemand>(conciergeServiceDemandDto);
            await _conciergeServiceDemandRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            //await _fileOperationService.UploadFile(Convert.ToInt16(DemandTypes.ConciergeServiceDemand), entity.Id, conciergeServiceDemandDto.Notes);
        }

        public async Task AddVipServiceDemand(AddVipDemand vipServiceDemandDto)
        {
            var entity = _mapper.Map<VipServiceDemand>(vipServiceDemandDto);
            await _vipServiceDemandRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();

        }


        public async Task<List<GetAllConciergeServiceDto>> GetAllConciergeServices(int userId)
        {
            var conciergeServices = await _conciergeServiceDemandRepository.GetAll()
               .Include(c => c.User)
               .Where(x => x.UserId == userId)
               .OrderByDescending(x => x.Status).ThenByDescending(y => y.CreatedDate)
               .ToListAsync();

            var conciergeServicesDto = _mapper.Map<List<GetAllConciergeServiceDto>>(conciergeServices);

            if (conciergeServicesDto != null && conciergeServicesDto.Count() > 0)
                return conciergeServicesDto;
            else
                return null;
        }

        public async Task<List<OrderList>> GetAllOrdersService(int userId)
        {
            var model = new List<OrderList>();

            var flowerOrders = _context.FlowerOrders.Where(x => x.UserId == userId).ToList();
            if (flowerOrders != null)
            {
                foreach (var item in flowerOrders)
                {
                    var childModel = new OrderList
                    {
                        Category = "Flower Order",
                        OrderDate = item.CreatedDate,
                        Status = item.Status
                    };

                    model.Add(childModel);
                }
            }



            var fuelOrders = _context.FuelPurchaseDemands.Where(x => x.UserId == userId).ToList();
            if (fuelOrders != null)
            {
                foreach (var item in fuelOrders)
                {
                    var childModel = new OrderList
                    {
                        Category = "Fuel Order",
                        OrderDate = item.CreatedDate,
                        Status = item.Status
                    };

                    model.Add(childModel);
                }
            }

            var provisionOrders = _context.ProvisionOrders.Where(x => x.UserId == userId).ToList();
            if (provisionOrders != null)
            {
                foreach (var item in provisionOrders)
                {
                    var childModel = new OrderList
                    {
                        Category = "Provision Order",
                        OrderDate = item.CreatedDate,
                        Status = item.Status
                    };

                    model.Add(childModel);
                }
            }
           

            return model;
        }


        public async Task<List<OrderList>> GetAllRequetsService(int userId)
        {
            var model = new List<OrderList>();

            var flowerOrders = _context.ExcursionDemands.Where(x => x.UserId == userId).ToList();
            if (flowerOrders != null)
            {
                foreach (var item in flowerOrders)
                {
                    var childModel = new OrderList
                    {
                        Category = "Excursion Request",
                        OrderDate = item.CreatedDate,
                        Status = item.Status
                    };

                    model.Add(childModel);
                }
            }



            var fuelOrders = _context.VipDemand.Where(x => x.UserId == userId).ToList();
            if (fuelOrders != null)
            {
                foreach (var item in fuelOrders)
                {
                    var childModel = new OrderList
                    {
                        Category = "Vip Transfer Request",
                        OrderDate = item.CreatedDate,
                        Status = item.Status
                    };

                    model.Add(childModel);
                }
            }

            var provisionOrders = _context.TechnicalAssitanceandSparePartOrders.Where(x => x.UserId == userId).ToList();
            if (provisionOrders != null)
            {
                foreach (var item in provisionOrders)
                {
                    var childModel = new OrderList
                    {
                        Category = "Technical Assistant Request",
                        OrderDate = item.CreatedDate,
                        Status = item.Status
                    };

                    model.Add(childModel);
                }
            }

            var cons = _context.ConciergeServiceDemands.Where(x => x.UserId == userId).ToList();
            if (provisionOrders != null)
            {
                foreach (var item in provisionOrders)
                {
                    var childModel = new OrderList
                    {
                        Category = "Concierge Service Request",
                        OrderDate = item.CreatedDate,
                        Status = item.Status
                    };

                    model.Add(childModel);
                }
            }


            var sec = _context.SecurityServiceDemands.Where(x => x.UserId == userId).ToList();
            if (provisionOrders != null)
            {
                foreach (var item in provisionOrders)
                {
                    var childModel = new OrderList
                    {
                        Category = "Security Service Request",
                        OrderDate = item.CreatedDate,
                        Status = item.Status
                    };

                    model.Add(childModel);
                }
            }



            return model;
        }

        public async Task<GetlConciergeServiceByIdandUserIdDto> GetlConciergeServiceById(int id, int userId)
        {
            var conciergeService = await _conciergeServiceDemandRepository.Where(x => x.Id == id && x.UserId == userId).FirstOrDefaultAsync();
            var conciergeServiceDto = _mapper.Map<GetlConciergeServiceByIdandUserIdDto>(conciergeService);
            conciergeServiceDto.Notes = await _fileOperationService.GetFilesById(Convert.ToInt16(DemandTypes.ConciergeServiceDemand), id);
            //if (conciergeServiceDto.Notes.Count() < 0 || conciergeServiceDto.Notes == null) test et!!

            if (conciergeServiceDto != null)
                return conciergeServiceDto;
            else
                return null;
        }
    }
}
