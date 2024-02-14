using BegumYatch.Core.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Models.Demands
{
    public class CheckIn
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Loa { get; set; }
        public string? Beam { get; set; }
        public string? Draft { get; set; }
        public string? GrossTonnage { get; set; }
        public string? NetTonnage { get; set; }
        public DateTime? CheckInDate { get; set; }
        public string? ArrivalPort { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string? DeparturePort { get; set; }
        public string? YatchType { get; set; }
        public string? YatchName { get; set; }
        public string? Flag { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}
