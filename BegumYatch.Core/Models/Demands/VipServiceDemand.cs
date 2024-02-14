using BegumYatch.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Models.Demands
{
    public class VipServiceDemand:CommonEntity
    {
        public string? NameSurname { get; set; }
        public DateTime? TransferDate { get; set; }
        public DateTime? TransferTime { get; set; }
        public int NumberOfPeople { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string? Luggage { get; set; }
        public string? AccountOps { get; set; }
        public string? VehicleType { get; set; }
        public string? Notes { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }
        public int CheckInId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
