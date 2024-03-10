namespace BegumYatch.Core.Models.AdminPanel.DemandModel
{
    public record VipTransferDemandModel : BaseDemandAndOrderModel
    {
        public DateTime? TransferDate { get; init; }
        public int? NumberOfPeople { get; init; }
        public string? From { get; init; }
        public string? To { get; init; }
        public string? Luggage { get; init; }
        public string? AccountOps { get; init; }
        public string? VehicleType { get; init; }
        public string? Notes { get; init; }
    }
}
