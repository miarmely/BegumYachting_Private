namespace BegumYatch.Core.QueryParameters
{
    public record DemandParamsForAnsweredFuelPurchase : PagingParams
    {
        public bool AcceptedOrRejected { get; init; }
    }
}
