namespace BegumYatch.Core.Models.AdminPanel
{
    public record FuelPurchaseDemandModel : BaseDemandAndOrderModel
    {
        public string? RequestedFuel { get; init; } //Talep edilen yakıt tipi
        public bool? IsDutyPaid { get; init; } //gümrüklümü gümrüksüz mü
        public float? Mgo { get; init; } //yakıt litresi
        public float? Ago { get; init; } // flashpoint derecesi
        public DateTime FuelSupplyDate { get; init; }
        public string? FuelSupplyPort { get; init; }
        public string? FuelType { get; init; }
        public int? CheckInId { get; init; }
        public string? Notes { get; init; }
        public bool? Status { get; init; }
    }
}