using BegumYatch.Core.Enums;
using BegumYatch.Core.Enums.AdminPanel;
using BegumYatch.Core.Models.Attributes;
using Entities.Attributes;
using MiarServices.Attributes;

namespace BegumYatch.Core.DTOs.User
{
    public record UserDtoForUpdate
    {
        public string? NameSurname { get; init; }

        [MiarLength(10, 10, "Telefon", "Phone")]
        [MiarPhone]
        public string? PhoneNumber { get; init; }

        [MiarLength(1, 150, "Email", "Email")]
        [MiarEnglishChars(new char[] { '.', '-', '_', '@' }, "Email", "Email")]
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
        public Roles? RoleName { get; init; }

        [MiarLength(6, 16, "Şifre", "Password")]
        [MiarEnglishChars(
            new char[] { '.', ',', '!', '?', '-', ':', ';' }, "Şifre", "Password")]
        [MiarPassword(true, true, true, null, 1, 1, 1, "Şifre", "Password")]
        public string? Password { get; init; }
    }
}