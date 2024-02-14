using BegumYatch.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.User
{
    public class UserUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        [Display(Name = "E-Posta :")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Kullanıcı Adı  boş bırakılamaz.")]
        [Display(Name = "Kullanıcı Adı :")]
        public string? UserName { get; set; }


        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        [MinLength(5, ErrorMessage = "{0} {1} karakterden küçük olmamalıdır.")]
        [DataType(DataType.Password)]
        [DisplayName("Şifre")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(50, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden küçük olmamalıdır.")]
        [Display(Name = "İsim Soyisim :")]
        public string? NameSurname { get; set; }

        [DisplayName("Telefon Numarası")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(13, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")] // +905555555555
        [MinLength(13, ErrorMessage = "{0} {1} karakterden küçük olmamalıdır.")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        [DisplayName("Bayrak: ")]
        public string? Flag { get; set; }
        [DisplayName("Yeni Tip Pasaport: ")]

        public string? NewPassportNo { get; set; } //7 haneli 
        [DisplayName("Eski Tip Pasaport: ")]
        public string? OldPassportNo { get; set; } //9 haneli
        [DisplayName("Rütbe: ")]
        public string? Rank { get; set; }

        [DisplayName("Pasaport Veriliş Tarihi: ")]
        [DataType(DataType.DateTime)]
        public DateTime? DateOfIssue { get; set; }
        [DisplayName("Pasaport Bitiş Tarihi: ")]
        [DataType(DataType.DateTime)]
        public DateTime? PassPortExpiry { get; set; }
        [DisplayName("Doğum Tarihi: ")]
        [DataType(DataType.DateTime)]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName("Doğum Yeri: ")]
        public string? PlaceOfBirth { get; set; }
        [DisplayName("Cinsiyet: ")]
        public Gender? Gender { get; set; }
        [DisplayName("Personel/Mürettebat: ")]
        public bool? IsPersonel { get; set; }
        [DisplayName("Yat Tipi: ")]
        public YacthType? YacthType { get; set; }
        [DisplayName("Yat İsmi: ")]
        public string? YacthName { get; set; }
    }
}
