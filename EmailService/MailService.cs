using Common.Shared.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailService
{
    /// <summary>
    /// A service for sending emails using SMTP. It utilizes the MailKit library to handle email composition and transmission.
    /// </summary>
    public class MailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public MailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<string> GetEmailTemplate(string templateName, CancellationToken ct)
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "NewLoginEmail.html");
            return await File.ReadAllTextAsync(templatePath, ct);
        }

        /// <inheritdoc />
        public async Task SendEmailAsync(string to, string subject, string body, CancellationToken ct)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);

            await client.AuthenticateAsync(_settings.Username, _settings.Password, ct);
            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);
        }
    }
}
