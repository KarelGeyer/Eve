using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entites
{
    public class GDPR
    {
        public int UserId { get; set; }
        public DateTime TosAcceptAt { get; set; }
        public DateTime PrivacyPolicyAcceptedAt { get; set; } = DateTime.UtcNow;
        public string TosVersion { get; set; } = string.Empty;
    }
}
