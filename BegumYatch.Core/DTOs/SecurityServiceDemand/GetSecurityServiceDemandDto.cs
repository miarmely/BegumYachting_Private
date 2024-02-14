using BegumYatch.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.SecurityServiceDemand
{
    public class GetSecurityServiceDemandDto
    {
        public RequestedServiceType RequestedServiceTypes { get; set; }
        public bool Status { get; set; }
        public List<string>? Notes { get; set; }
    }
}
