using BegumYatch.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.FlowerOrder
{
    public class AddFlowerOrderDto
    {
        public string flowerAndArragmentsInfo { get; set; }
        public string supplyPort { get; set; }
        public DateTime supplyDate { get; set; }
        public AccountTypes accountTypes { get; set; }
        public List<IFormFile>? note { get; set; }
        public bool status { get; set; }
        public int userId { get; set; }
    }
}
