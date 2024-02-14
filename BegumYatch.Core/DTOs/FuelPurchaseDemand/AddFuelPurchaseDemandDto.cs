using BegumYatch.Core.Models.FileOperations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.FuelPurchaseDemand
{
    public class AddFuelPurchaseDemandDto
    {
     
        //public List<IFormFile>? notes { get; set; }
        public bool status { get; set; }

        public bool isDutyPaid { get; set; } //gümrüklümü gümrüksüz mü
        public float requestedFuel { get; set; } //Talep edilen yakıt tipi  
        public float mgo { get; set; } //yakıt listesi
        public float ago { get; set; } // flashpoint derecesi
        public string? fuelSupplyDate { get; set; } //yakıt ikmal tarihi
        public string? fuelSupplyPort { get; set; } //yakıt ikmal yeri
        public string? nameSurname { get; set; } //yakıt ikmal yeri
        public string? notes { get; set; } //yakıt ikmal yeri
        public int userId { get; set; }
        public int CheckInId { get; set; }
        public string  FuelType { get; set; }

    }
}
