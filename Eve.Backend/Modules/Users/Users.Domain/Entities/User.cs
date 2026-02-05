using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual UserSettings? Settings { get; set; } = null;
        public virtual UserIdentity? Identity { get; set; } = null;
        public virtual GDPR? GDPR { get; set; } = null;
    }
}
