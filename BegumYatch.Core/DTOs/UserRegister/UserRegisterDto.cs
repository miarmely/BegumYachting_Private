using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.UserRegister
{
    public class UserRegisterDto
    {
       // [Required(ErrorMessage ="Lütfen Ad Soyad Giriniz.")]
        public string NameSurname { get; set; }
        //[Required(ErrorMessage = "Lütfen Email Giriniz.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        //[Required(ErrorMessage = "Lütfen Telefon Numarası Giriniz.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

     //   [Required(ErrorMessage = "Lütfen Şifre Giriniz.")]
        public string Password { get; set; }
    }
}
