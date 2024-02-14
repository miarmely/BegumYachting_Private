using BegumYatch.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Models.User
{
    public class MailOtp
    {

        [Key]
        public int OtpId { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        public DateTime DateAdded { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }


    }
}