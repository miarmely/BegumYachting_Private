using BegumYatch.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string subject, string body,string toEmail);
    }
}
