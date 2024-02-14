using BegumYatch.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.FlowerOrder
{
    public class GetAllFlowerOrdersDto
    {
        public string FlowerAndArragmentsInfo { get; set; }
        public AccountTypes AccountTypes { get; set; }     
        public bool Status { get; set; }
    }
}
