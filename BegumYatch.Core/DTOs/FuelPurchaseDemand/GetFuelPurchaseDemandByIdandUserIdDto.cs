using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.FuelPurchaseDemand
{
    public class GetFuelPurchaseDemandByIdandUserIdDto
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public bool IsDutyPaid { get; set; } //gümrüklümü gümrüksüz mü
        public float RequestedFuel { get; set; } //Talep edilen yakıt tipi  
        public float FuelLiters { get; set; } //yakıt listesi
        public float FlashPoint { get; set; } // flashpoint derecesi
        public DateTime FuelSupplyDate { get; set; } //yakıt ikmal tarihi
        public string FuelSupplyPort { get; set; } //yakıt ikmal yeri
        public string FuelHoseConnectPort { get; set; } //Yakıt Hortumu Bağlantı Noktası Tipi
        public float FuelHoseConnectSize { get; set; } //Yakıt Hortumu Bağlantı Noktası Ölçüsü
        public List<string>? Notes { get; set; }
    }
}
