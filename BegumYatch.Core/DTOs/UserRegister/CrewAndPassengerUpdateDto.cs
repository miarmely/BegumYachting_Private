using BegumYatch.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.UserRegister
{
    public class CrewAndPassengerUpdateDto
    {
        public int Id { get; set; } //0 dan büyük
        public string? NameSurname { get; set; } //max 100 karakter olsun 
        public string? NewPassportNo { get; set; } //7 haneli 
        public string? OldPassportNo { get; set; } //9 haneli
        public string? Rank { get; set; } // üç haneden fazla 
        public DateTime DateOfIssue { get; set; }  //pasaport verilme tarihi şuandan küçük olmalıdır ve büyük olmamalıdır.
        public DateTime PassPortExpiry { get; set; }  //şuandan büyük olmalıdır.
        public string? Nationality { get; set; }
        public DateTime DateOfBirth { get; set; } 
        public string? PlaceOfBirth { get; set; } // en az 3 karakter
        public Gender Gender { get; set; }
        public bool IsPersonel { get; set; }
    }
}
