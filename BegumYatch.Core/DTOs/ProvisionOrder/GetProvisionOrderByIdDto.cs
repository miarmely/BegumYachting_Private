using BegumYatch.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.ProvisionOrder
{
    public class GetProvisionOrderByIdDto
    {
        public string ProvisionOrderInfo { get; set; }
        public string SupplyPort { get; set; }
        public DateTime SupplyDate { get; set; }
        public AccountTypes AccountTypes { get; set; }
        public bool Status { get; set; }
        public List<string>? Notes { get; set; }
    }
}
