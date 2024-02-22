using BegumYatch.Core.Enums;
using Entities.Attributes;

namespace BegumYatch.Core.DTOs.User
{
    public record UserDtoForUpdate
    {
        public string? NameSurname { get; init; }

        [MiarLength(10, 10, "Telefon", "Phone")]
        [MiarPhone]
        public string? PhoneNumber { get; init; }

        [MiarLength(1, 150, "Email", "Email")]
        [MiarEmail]
        public string? Email { get; init; }

        public string? Flag { get; init; }
        public string? NewPassportNo { get; init; } // 7 haneli 
        public string? OldPassportNo { get; init; } // 9 haneli
        public string? Rank { get; init; }
        public DateTime? DateOfIssue { get; init; }
        public DateTime? PassPortExpiry { get; init; }
        public string? Nationality { get; init; }
        public DateTime? DateOfBirth { get; init; }
        public string? PlaceOfBirth { get; init; }
        public string? Gender { get; init; }
        public YacthType? YacthType { get; init; }
        public string? YacthName { get; init; }
        public bool? IsPersonel { get; init; }
        public string? Password { get; init; }
    }
}