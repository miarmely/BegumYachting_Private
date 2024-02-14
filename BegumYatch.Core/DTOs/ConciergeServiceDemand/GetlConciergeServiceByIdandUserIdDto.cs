using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.ConciergeServiceDemand
{
    public class GetlConciergeServiceByIdandUserIdDto
    {
        public string RestaurantName { get; set; }
        public DateTime ConciergeDateTime { get; set; }
        public int PeopleNumber { get; set; }
        public string TransportationPreference { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string>? Notes { get; set; }
    }

    public class OrderList
    {
        public bool Status { get; set; }
        public DateTime OrderDate { get; set; }
        public string Category { get; set; }
       
    }

}
