using BegumYatch.Core.Models.Attributes;
using Entities.Attributes;
using MiarServices.Attributes;
using System.ComponentModel.DataAnnotations;


namespace BegumYatch.Core.DTOs.AdminPanel.Login
{
    public record LoginDtoForResetPassword
    {
        [Required] 
        public string UserId { get; init; }

        [Required] 
        public string TokenForResetPassword { get; init; }

        [Required]
        [MiarLength(6, 16, "Şifre", "Password")]
        [MiarEnglishChars(
            new char[] { '.', ',', '!', '?', '-', ':', ';' }, "Şifre", "Password")]
        [MiarPassword(true, true, true, null, 1, 1, 1, "Şifre", "Password")]
        public string NewPassword { get; init; }
    }
}
