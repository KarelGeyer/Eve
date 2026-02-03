using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entites
{
    public class UserIdentity
    {
        public int UserId { get; set; }
        public string GoogleSubId { get; set; } = string.Empty;
        public string AppleSubId { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string DeviceToken { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

    }
}
