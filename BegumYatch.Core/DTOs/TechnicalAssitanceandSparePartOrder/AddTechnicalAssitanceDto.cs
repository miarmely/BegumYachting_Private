using BegumYatch.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.TechnicalAssitanceandSparePartOrder
{
    public class AddTechnicalAssitanceDto
    {
        public string? RequestedService { get; set; }
        public string? SparePart { get; set; }
        public string? Notes { get; set; }
        public string? NameSurname { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }
        public int CheckInId { get; set; }
    }
}
