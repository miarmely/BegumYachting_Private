using BegumYatch.Core.Enums;
using BegumYatch.Core.Models.FileOperations;
using BegumYatch.Core.Models.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Models.Demands
{
    public class FuelPurchaseDemand:CommonEntity
    {
        
        //public List<IFormFile>? Notes { get; set; }
        public string? RequestedFuel { get; set; } //Talep edilen yakıt tipi
        public bool? IsDutyPaid { get; set; } //gümrüklümü gümrüksüz mü
        public float? Mgo { get; set; } //yakıt litresi
        public float? Ago { get; set; } // flashpoint derecesi
        public DateTime? FuelSupplyDate { get; set; } //yakıt ikmal tarihi
        public string? FuelSupplyPort { get; set; } //yakıt ikmal yeri
        public string? NameSurname { get; set; } //yakıt ikmal yeri
        public string? Notes { get; set; } //yakıt ikmal yeri
        public string? FuelType { get; set; } //yakıt ikmal yeri
        public int UserId { get; set; }
        public virtual AppUser User { get; set; }

        public int? CheckInId { get; set; }
    }
}
