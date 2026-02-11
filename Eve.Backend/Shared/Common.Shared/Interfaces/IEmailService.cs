using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Common.Shared.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// A method for sending an email asynchronously.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="ct"></param>
        /// <returns>A Task</returns>
        Task SendEmailAsync(string to, string subject, string body, CancellationToken ct);

        /// <summary>
        /// A method for retrieving an email template asynchronously.
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="ct"></param>
        /// <returns>A Task</returns>
        Task<string> GetEmailTemplate(string templateName, CancellationToken ct);
    }
}
