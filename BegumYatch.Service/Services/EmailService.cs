using BegumYatch.Core.Models.EmailSettings;
using BegumYatch.Core.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;


namespace BegumYatch.Service.Services
{
	public class EmailService : IEmailService
	{
		private readonly EmailSettings _emailSettings;

		public EmailService(IOptions<EmailSettings> options)
		{
			_emailSettings = options.Value;
		}

		public async Task SendEmailAsync(
			string subject,
			string body,
			string toEmail)
		{
			var smptClient = new SmtpClient();
			smptClient.Host = _emailSettings.Host;
			smptClient.DeliveryMethod = SmtpDeliveryMethod.Network;
			smptClient.UseDefaultCredentials = false;
			smptClient.Port = 587;
			smptClient.Credentials = new NetworkCredential(
				_emailSettings.Email,
				_emailSettings.Password);
			smptClient.EnableSsl = true;

			var mailMessage = new MailMessage();
			mailMessage.From = new MailAddress(_emailSettings.Email);
			mailMessage.To.Add(toEmail);
			mailMessage.Subject = subject;
			mailMessage.Body = body;
			mailMessage.IsBodyHtml = true;

			await smptClient.SendMailAsync(mailMessage);
		}
	}
}