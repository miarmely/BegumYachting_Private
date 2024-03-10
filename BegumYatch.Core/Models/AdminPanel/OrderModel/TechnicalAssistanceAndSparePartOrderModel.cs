namespace BegumYatch.Core.Models.AdminPanel.OrderModel
{
    public record TechnicalAssistanceAndSparePartOrderModel : BaseDemandAndOrderModel
    {
        public string? RequestedService { get; init; }
        public string? SparePart { get; init; }
        public string? Notes { get; init; }
    }
}
