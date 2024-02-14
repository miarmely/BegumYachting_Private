using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BegumYatch.Core.ViewModels
{
    public class SignInViewModel
    {
        public SignInViewModel()
        { }

        public SignInViewModel(string email, string password)
        {
            Email = email;
            Password = password;
        }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        [Display(Name = "E-Posta :")]
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        [Display(Name = "Şifre :")]
        public string Password { get; set; } = null!;

        [Display(Name = "Beni Hatırla ")]
        public bool RememberMe { get; set; }
    }
}
