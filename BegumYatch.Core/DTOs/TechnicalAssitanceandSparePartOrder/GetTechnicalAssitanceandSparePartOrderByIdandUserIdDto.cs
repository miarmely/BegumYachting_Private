using BegumYatch.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.TechnicalAssitanceandSparePartOrder
{
    public class GetTechnicalAssitanceandSparePartOrderByIdandUserIdDto
    {
        public string ServiceAndSparePartInfo { get; set; }
        public AccountTypes AccountTypes { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string>? Notes { get; set; }
    }
}
