using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.VipDemand
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace BegumYatch.Core.DTOs.VipDemand
    {
        public class AddVipDemand
        {

            public string? nameSurname { get; set; }
            public DateTime? transferDate { get; set; }
            public DateTime? transferTime { get; set; }
            public int numberOfPeople { get; set; }
            public string? from { get; set; }
            public string? to { get; set; }
            public string? luggage { get; set; }
            public string? accountOps { get; set; }
            public string? notes { get; set; }
            public string? vehicleType { get; set; }
            public bool status { get; set; }
            public int userId { get; set; }
            public int checkInId { get; set; }
        }
    }

}
