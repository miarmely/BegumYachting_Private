using BegumYatch.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Models.BerthRezervation
{
    public class BerthRezervation : CommonEntity
    {
        public string? NameSurname { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string? TypeOfYacht { get; set; }
        public string MarinaName { get; set; }

        public string RequestShorePower { get; set; }
        public string AccountOps { get; set; }
        public string Notes { get; set; }
        public bool Status { get; set; }
        public int CheckInId { get; set; }
    }
}
