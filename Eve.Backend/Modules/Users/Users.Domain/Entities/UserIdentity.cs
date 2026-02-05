using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class UserIdentity
    {
        public int UserId { get; set; }
        public string GoogleSubId { get; set; } = null!;
        public string AppleSubId { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string DeviceToken { get; set; } = null!;
        public string DeviceType { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
