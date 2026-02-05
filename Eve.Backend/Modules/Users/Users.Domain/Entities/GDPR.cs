using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class GDPR
    {
        public int UserId { get; set; }
        public DateTime TosAcceptAt { get; set; }
        public DateTime PrivacyPolicyAcceptedAt { get; set; } = DateTime.UtcNow;
        public string? TosVersion { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
