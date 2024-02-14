using BegumYatch.Core.Enums;
using BegumYatch.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Models.Orders
{
    public class FlowerOrder:CommonEntity
    {
        public string FlowerAndArragmentsInfo { get; set; }
        public string SupplyPort { get; set; }
        public DateTime SupplyDate { get; set; }
        public AccountTypes AccountTypes { get; set; }
        public int UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
