namespace BegumYatch.Core.DTOs.User
{
    public record UserDtoForDelete
    {
        public IEnumerable<string> Emails { get; init; }
    }
}
