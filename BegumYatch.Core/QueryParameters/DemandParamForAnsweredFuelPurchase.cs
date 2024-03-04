using BegumYatch.Core.Enums.AdminPanel;

namespace BegumYatch.Core.QueryParameters
{
    public record DemandParamsForAnsweredFuelPurchase : PagingParams
    {
        public DemandStatus DemandStatus { get; init; }
    }
}
