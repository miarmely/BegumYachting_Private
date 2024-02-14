
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.ExcursionDemand
{
    public class GetExcursionDemandByIdandUserIdDto
    {
        public string ExcursionType { get; set; }
        public DateTime ExcursionDateTime { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int PeopleNumber { get; set; }
        public string VehicleType { get; set; }
        public string FirstLanguage { get; set; }
        public string OptionalLanguage { get; set; }
        public bool Status { get; set; }
        public List<string>? Notes { get; set; }
    }
}
