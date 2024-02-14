using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.ConciergeServiceDemand
{
    public class AddConciergeServiceDemandDto
    {
        public string? restaurantName { get; set; }
        public string? conciergeDateTime { get; set; }
        public int peopleNumber { get; set; }
        public string? transportationPreference { get; set; }
        public string? transferCarType { get; set; }

        
        public string? note { get; set; }
        public bool status { get; set; }
        public int userId { get; set; }


    }
}
