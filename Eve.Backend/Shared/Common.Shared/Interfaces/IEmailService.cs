using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Common.Shared.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, CancellationToken ct);
    }
}
