using BegumYatch.Core.Enums;
using BegumYatch.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Models.Orders
{
    public class ProvisionOrder:CommonEntity
    {
        public string? NameSurname { get; set; }
        public string? SupplyPort { get; set; }
        public DateTime? SupplyDate { get; set; }
        public string? AccountOps { get; set; }
        public string? Notes { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }
        public int? CheckInId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
