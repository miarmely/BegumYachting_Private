using BegumYatch.Core.Enums;


namespace BegumYatch.Core.Models.User
{
    public record MiarUser
    {
        public int Id { get; init; }
        public string? NameSurname { get; init; }
        public string? PhoneNumber { get; init; }
        public string? Email { get; init; }
        public string? Gender { get; init; }
        public string? Nationality { get; init; }
        public YacthType? YacthType { get; init; }
        public string? YacthName { get; init; }
        public string? Flag { get; init; }
        public string? NewPassportNo { get; init; } // 7 haneli 
        public string? OldPassportNo { get; init; } // 9 haneli
        public string? Rank { get; init; }
        public DateTime? DateOfIssue { get; init; }
        public DateTime? PassPortExpiry { get; init; }
        public DateTime? DateOfBirth { get; init; }
        public string? PlaceOfBirth { get; init; }
        public bool? IsPersonel { get; init; }
        public bool? IsDeleted { get; init; }
        public string RoleName { get; init; }
    }
}
