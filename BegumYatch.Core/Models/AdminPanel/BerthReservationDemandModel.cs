namespace BegumYatch.Core.Models.AdminPanel
{
    public record BerthReservationDemandModel : BaseDemandAndOrderModel
    {
        public DateTime CheckinDate { get; init; }
        public DateTime CheckoutDate { get; init; }
        public string? MarinaName { get; init; }
        public string? RequestShorePower { get; init; }  // required electric power
        public string? AccountOps { get; init; }
        public string? Notes { get; init; }
    }
}
