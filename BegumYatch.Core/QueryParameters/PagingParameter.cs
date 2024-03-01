namespace BegumYatch.Core.QueryParameters
{
    public record PagingParameter
    {
        public int PageSize{ get; init; }
        public int PageNumber { get; init; }
    }
}
