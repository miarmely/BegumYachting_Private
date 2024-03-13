using BegumYatch.Core.Enums;


namespace BegumYatch.Core.DTOs.User
{
    public class GetUsersDto
    {
        public string? NameSurname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Flag { get; set; }
        public string? NewPassportNo { get; set; } // 7 haneli 
        public string? OldPassportNo { get; set; } // 9 haneli
        public string? Rank { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public DateTime? PassPortExpiry { get; set; }
        public string? Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PlaceOfBirth { get; set; }
        public string? Gender { get; set; }
        public YacthType? YacthType { get; set; }
        public string? YacthName { get; set; }
        public bool? IsPersonel { get; set; }
    }
}