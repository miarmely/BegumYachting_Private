using BegumYatch.Core.Enums;


namespace BegumYatch.Core.Models.Demands.AdminPanel
{
    public record DemandDtoForAnsweredFuelPurchase
    {
        public int FormId { get; init; }  // demand id
        public int AnswererId { get; init; }
        public int UserId { get; init; }  // demand owner
        public string NameSurname { get; init; }
        public string YachtName { get; init; }
        public YacthType YachtType { get; init; }
        public string Flag { get; init; }
        public string? RequestedFuel { get; init; } //Talep edilen yakıt tipi
        public bool? IsDutyPaid { get; init; } //gümrüklümü gümrüksüz mü
        public float? Mgo { get; init; } //yakıt litresi
        public float? Ago { get; init; } // flashpoint derecesi
        public DateTime? FuelSupplyDate { get; init; }
        public string? FuelSupplyPort { get; init; }
        public string? FuelType { get; init; }
        public int? CheckInId { get; init; }
        public string? Notes { get; init; }
        public bool Status { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime AnsweredDate { get; init; }
    }
}