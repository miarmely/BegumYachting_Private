using BegumYatch.Core.Enums;
using BegumYatch.Core.Models.Demands;
using BegumYatch.Core.Models.Orders;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Models.User
{

    public class AppUser: IdentityUser<int>
    {
        public string? NameSurname { get; set; }
        public string? YacthName { get; set; }
        public YacthType? YacthType { get; set; }
        public string? Flag { get; set; }
        public bool IsUpdated { get; set; }
        public int? ConfirmCode { get; set; }
        public string? NewPassportNo { get; set; } // 7 haneli 
        public string? OldPassportNo { get; set; } // 9 haneli
        public string? Rank { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public DateTime? PassPortExpiry { get; set; }
        public string? Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PlaceOfBirth { get; set; }
        public string? Gender { get; set; }
        public bool? IsPersonel { get; set; }
        public bool  IsDeleted { get; set; }
        public virtual List<ConciergeServiceDemand> ConciergeServiceDemands 
            { get; set; }
        public virtual List<ExcursionDemand> ExcursionDemands { get; set; }
        public virtual List<FuelPurchaseDemand> FuelPurchaseDemands { get; set; }
        public virtual List<SecurityServiceDemand> SecurityServiceDemands 
            { get; set; }
        public virtual List<ProvisionOrder> ProvisionOrders { get; set; }
        public virtual List<FlowerOrder> FlowerOrders { get; set; }
        public virtual List<TechnicalAssitanceandSparePartOrder> TechnicalAssitanceandSparePartOrders { get; set; }
    }
}
