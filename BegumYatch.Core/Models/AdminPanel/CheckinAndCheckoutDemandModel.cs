namespace BegumYatch.Core.Models.AdminPanel
{
    public record CheckinAndCheckoutDemandModel : BaseDemandAndOrderModel
    {
        public DateTime CheckInDate { get; init; }
        public string ArrivalPort { get; init; }
        public DateTime CheckOutDate { get; init; }
        public string DeparturePort { get; init; }
    }
}