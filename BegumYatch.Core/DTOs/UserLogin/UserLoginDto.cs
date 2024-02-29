using System.ComponentModel.DataAnnotations;

namespace BegumYatch.Core.DTOs.UserLogin
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Lütfen Email Giriniz.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lütfen Şifre Giriniz.")]
        public string Password { get; set; }
    }
}
