using BegumYatch.Core.Enums;


namespace BegumYatch.Core.DTOs.User
{
    public class GetPersonelInfoByIdDto
    {
        public int Id { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Flag { get; set; }
        public string YacthName { get; set; }
        public YacthType YacthType { get; set; }
        public int ConfirmCode { get; set; }
    }
}
