using BegumYatch.Core.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Models.Demands
{
    public class ConciergeServiceDemand : CommonEntity
    {
        public string RestaurantName { get; set; }
        public DateTime ConciergeDateTime { get; set; }
        public int PeopleNumber { get; set; }
        public string TransportationPreference { get; set; }
        public string TransferCarType { get; set; }
        public int UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
