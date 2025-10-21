using ActiviGo.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace ActiviGo.Application.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public SmtpEmailService(IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("EmailSettings");
            _smtpHost = emailSettings["SmtpHost"] ?? string.Empty;
            var port = emailSettings["SmtpPort"];
            _smtpPort = int.TryParse(port, out var parsed) ? parsed : 0;
            _smtpUser = emailSettings["SmtpUser"] ?? string.Empty;
            _smtpPass = emailSettings["SmtpPass"] ?? string.Empty;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // Basic validation; silently skip if not configured to avoid breaking core flow
            if (string.IsNullOrWhiteSpace(to) || string.IsNullOrWhiteSpace(_smtpHost) || _smtpPort <= 0 || string.IsNullOrWhiteSpace(_smtpUser))
            {
                return;
            }

            using var client = new SmtpClient(_smtpHost, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true
            };

            var mail = new MailMessage(_smtpUser, to, subject, body)
            {
                IsBodyHtml = true
            };
            await client.SendMailAsync(mail);
        }
    }
}
