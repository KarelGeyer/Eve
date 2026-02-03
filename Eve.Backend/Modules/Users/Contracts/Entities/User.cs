using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.Entites
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual UserSettings? Settings { get; set; } = null;
        public virtual UserIdentity? Identity { get; set; } = null;
        public virtual GDPR? GDPR { get; set; } = null;
        public virtual Audit Audit { get; set; } = null;
    }
}
