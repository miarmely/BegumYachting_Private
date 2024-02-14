using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.FuelPurchaseDemand
{
    public class GetAllFuelPurchaseDemandsDto
    {
        public bool Status { get; set; }
        public bool IsDutyPaid { get; set; } //gümrüklümü gümrüksüz mü
    }
}
