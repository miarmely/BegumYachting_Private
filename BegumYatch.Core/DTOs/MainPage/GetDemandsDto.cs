﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.MainPage
{
    public class GetDemandsDto
    {
        public string DemandName { get; set; }
        public string DemandDescription { get; set; }
        public bool DemandStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
