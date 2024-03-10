namespace BegumYatch.Core.Models.AdminPanel.DemandModel
{
    public record SecurityAndProtectionServiceDemandModel : BaseDemandAndOrderModel
    {
        public string? RequestedService { get; init; }
        public string? Notes { get; init; }
    }
}
