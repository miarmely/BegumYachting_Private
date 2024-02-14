using BegumYatch.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BegumYatch.Core.DTOs.UserRegister
{
    public class PersonelInfoDto
    {
        public int Id { get; set; }
        public string YacthName { get; set; }
        public string Flag { get; set; }
        public string YacthType { get; set; }
        public string Email { get; set; }
        public int ConfirmCode { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ReturnResponseModel
    {
        public int Id { get; set; } = 0;
        public string? Email { get; set; }
        public string? NewEmail { get; set; }
        public int? ConfirmCode { get; set; } 
        public string? ReturnMessage { get; set; }
        public string YacthName { get; set; }
        public string Flag { get; set; }
        public YacthType? YacthType { get; set; }
    }

}
