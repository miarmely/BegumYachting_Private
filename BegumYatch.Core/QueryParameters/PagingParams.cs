namespace BegumYatch.Core.QueryParameters
{
    public record PagingParams
    {
        public int PageSize{ get; init; }
        public int PageNumber { get; init; }
    }
}
