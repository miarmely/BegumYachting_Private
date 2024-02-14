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
    public class SendConfirmCodeUpdateEmail
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string YacthName { get; set; }
        public string Flag { get; set; }
        public YacthType YacthType { get; set; }
    }
    public class VerifyConfirmCode
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string YacthName { get; set; }
        public string Flag { get; set; }
        public YacthType YacthType { get; set; }
        public int ConfirmCode { get; set; }
    }
}
