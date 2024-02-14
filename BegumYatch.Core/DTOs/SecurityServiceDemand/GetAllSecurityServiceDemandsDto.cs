using BegumYatch.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.SecurityServiceDemand
{
    public class GetAllSecurityServiceDemandsDto
    {
        public RequestedServiceType RequestedServiceTypes { get; set; }
      //  public List<IFormFile> Notes { get; set; }
        public bool Status { get; set; }
    }
}
