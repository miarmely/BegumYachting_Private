namespace BegumYatch.Core.QueryParameters
{
    public record UserParamsForDisplayByFiltering
    {
        public string? Email { get; init; }
        public string? Phone { get; init; }
        public bool CheckIsDeleted { get; init; } = true;
    }

    public record UserParamsForDisplayById
    {
        public int UserId { get; init; }
        public bool CheckIsDeleted { get; init; } = true;
    }
}