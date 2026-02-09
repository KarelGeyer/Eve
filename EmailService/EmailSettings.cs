using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService
{
    /// <summary>
    /// A class representing the configuration settings required for sending emails, including SMTP server details and sender information.
    /// </summary>
    public class EmailSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
    }
}
