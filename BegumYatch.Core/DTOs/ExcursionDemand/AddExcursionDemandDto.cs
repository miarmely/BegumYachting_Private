using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.ExcursionDemand
{
    public class AddExcursionDemandDto
    {

        public string?  nameSurname { get; set; }
        public DateTime? checkInDate { get; set; }
        public DateTime? checkInTime { get; set; }
        public int numberOfPeople { get; set; }
        public string? from { get; set; }
        public string? to { get; set; }
        public string? vehicleType { get; set; }
        public string? firstLanguage { get; set; }
        public string? secondLanguage { get; set; }
        public string? accountOps { get; set; }
        public string? notes { get; set; }
        public bool status { get; set; }
        public int? userId { get; set; }
        public int? checkInId { get; set; }
    }
}
