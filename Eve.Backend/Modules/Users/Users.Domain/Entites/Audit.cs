using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entites
{
    public class Audit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Message { get; set; } = string.Empty;
        public string Entity { get; set; } = string.Empty;
    }
}
