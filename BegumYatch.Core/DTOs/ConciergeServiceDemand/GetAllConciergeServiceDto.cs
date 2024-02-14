using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.ConciergeServiceDemand
{
    public class GetAllConciergeServiceDto
    {
        public string RestaurantName { get; set; }
        public bool Status { get; set; }
    }
}
