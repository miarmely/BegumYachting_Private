namespace BegumYatch.Core.Models.AdminPanel
{
    public record SecurityAndProtectionServiceDemandModel : BaseDemandAndOrderModel
    {
        public string? RequestedService { get; init; }
        public string? Notes { get; init; }
    }
}
