using BegumYatch.Core.Enums;
using BegumYatch.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Models.Demands
{
    public class ExcursionDemand : CommonEntity
    {
        public string? NameSurname { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckInTime { get; set; }
        public int NumberOfPeople { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string? VehicleType { get; set; }
        public string? FirstLanguage { get; set; }
        public string? SecondLanguage { get; set; }
        public string? AccountOps { get; set; }
        public string? Notes { get; set; }
        public bool Status { get; set; }
        public int CheckInId { get; set; }
        public int UserId { get; set; }
        public virtual AppUser User { get; set; }

    }
}
