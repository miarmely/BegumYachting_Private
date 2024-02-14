using AutoMapper;
using BegumYatch.Core.DTOs.BerthRezervation;
using BegumYatch.Core.DTOs.ConciergeServiceDemand;
using BegumYatch.Core.DTOs.ExcursionDemand;
using BegumYatch.Core.DTOs.FlowerOrder;
using BegumYatch.Core.DTOs.FuelPurchaseDemand;
using BegumYatch.Core.DTOs.MainPage;
using BegumYatch.Core.DTOs.ProvisionOrder;
using BegumYatch.Core.DTOs.Role;
using BegumYatch.Core.DTOs.RoleCreate;
using BegumYatch.Core.DTOs.SecurityServiceDemand;
using BegumYatch.Core.DTOs.TechnicalAssitanceandSparePartOrder;
using BegumYatch.Core.DTOs.User;
using BegumYatch.Core.DTOs.UserLogin;
using BegumYatch.Core.DTOs.UserRegister;
using BegumYatch.Core.DTOs.VipDemand.BegumYatch.Core.DTOs.VipDemand;
using BegumYatch.Core.Models.BerthRezervation;
using BegumYatch.Core.Models.Demands;
using BegumYatch.Core.Models.Orders;
using BegumYatch.Core.Models.Role;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<AppUser, UserRegisterDto>().ReverseMap();
            CreateMap<AppUser, CrewAndPassengerUpdateDto>().ReverseMap();
            CreateMap<AppUser, GetPersonelInfoByIdDto>().ReverseMap();
            CreateMap<AppUser, GetUsersDto>().ReverseMap();
            CreateMap<AppUser, UserAddViewModel>().ReverseMap();
            CreateMap<AppUser, UserUpdateDto>().ReverseMap();

            CreateMap<AppRole, RoleCreateDto>().ReverseMap();
            CreateMap<AppRole, RolesListDto>().ReverseMap();
            CreateMap<AppRole, RoleUpdateDto>().ReverseMap();

            CreateMap<FuelPurchaseDemand, AddFuelPurchaseDemandDto>().ReverseMap();
            CreateMap<FuelPurchaseDemand, GetAllFuelPurchaseDemandsDto>().ReverseMap();
            CreateMap<FuelPurchaseDemand, GetFuelPurchaseDemandByIdandUserIdDto>().ReverseMap();

            CreateMap<ConciergeServiceDemand, AddConciergeServiceDemandDto>().ReverseMap();
            CreateMap<ConciergeServiceDemand, GetAllConciergeServiceDto>().ReverseMap();
            CreateMap<ConciergeServiceDemand, GetlConciergeServiceByIdandUserIdDto>().ReverseMap();
            
            CreateMap<ExcursionDemand, AddExcursionDemandDto>().ReverseMap();
            CreateMap<ExcursionDemand, GetAllExcursionDemandDto>().ReverseMap();
            CreateMap<ExcursionDemand, GetExcursionDemandByIdandUserIdDto>().ReverseMap();

            CreateMap<VipServiceDemand, AddVipDemand>().ReverseMap();

            CreateMap<SecurityServiceDemand, AddSecurityServiceDemandDto>().ReverseMap();
            CreateMap<SecurityServiceDemand, GetAllSecurityServiceDemandsDto>().ReverseMap();
            CreateMap<SecurityServiceDemand, GetSecurityServiceDemandDto>().ReverseMap();
            

            CreateMap<FlowerOrder, AddFlowerOrderDto>().ReverseMap();
            CreateMap<FlowerOrder, GetAllFlowerOrdersDto>().ReverseMap();
            CreateMap<FlowerOrder, GetFlowerOrderByIdDto>().ReverseMap();
            

            CreateMap<ProvisionOrder, AddProvisionOrderDto>().ReverseMap();
            CreateMap<ProvisionOrder, GetAllProvisionOrdersDto>().ReverseMap();
            CreateMap<ProvisionOrder, GetProvisionOrderByIdDto>().ReverseMap();

            CreateMap<TechnicalAssitanceandSparePartOrder, AddTechnicalAssitanceDto>().ReverseMap();
            CreateMap<TechnicalAssitanceandSparePartOrder, GetAllTechnicalAssitanceandSparePartOrdersDto>().ReverseMap();
            CreateMap<TechnicalAssitanceandSparePartOrder, GetTechnicalAssitanceandSparePartOrderByIdandUserIdDto>().ReverseMap();

            CreateMap<BerthRezervation, AddBerthRezervationDto>().ReverseMap();
        }
    }
}
