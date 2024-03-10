namespace BegumYatch.Core.Models.AdminPanel.OrderModel
{
    public record ProvisionOrderModel : BaseDemandAndOrderModel
    {
        public DateTime? SupplyDate { get; init; }
        public string? SupplyPort { get; init; }
        public string? AccountOps { get; init; }
        public string? Notes { get; init; }
    }
}
