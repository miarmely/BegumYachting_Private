using BegumYatch.Core.Enums;


namespace BegumYatch.Core.Models.AdminPanel.OrderModel
{
    public record FlowerOrderModel : BaseDemandAndOrderModel
    {
        public string? FlowerAndArrangementsInfo { get; init; }
        public DateTime? SupplyDate { get; init; }
        public string? SupplyPort { get; init; }
        public AccountTypes? AccountTypes { get; init; }
    }
}
