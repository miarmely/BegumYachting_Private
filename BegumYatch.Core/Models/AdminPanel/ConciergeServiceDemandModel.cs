namespace BegumYatch.Core.Models.AdminPanel
{
    public record ConciergeServiceDemandModel : BaseDemandAndOrderModel
    {
        public string? RestaurantName { get; init; }
        public DateTime? ConciergeDate { get; init; }
        public int? NumberOfPeople { get; init; }
        public string? TransportationPreference { get; init; }
        public string? TransferCarType { get; init; }
    }
}
