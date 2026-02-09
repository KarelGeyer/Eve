using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Shared.Exceptions
{
    /// <summary>
    /// Represents an error that occurs when a security violation is detected for a specific entity type.
    /// </summary>
    /// <remarks>Use this exception to signal that an operation failed due to insufficient permissions or
    /// other security constraints related to an entity. The <see cref="EntityType"/> property identifies the entity
    /// type associated with the security violation, if available.</remarks>
    public class SecurityException : Exception
    {
        public string? EntityType { get; }

        public SecurityException(string entityType, string message)
            : base(message)
        {
            EntityType = entityType;
        }

        public SecurityException(string message)
            : base(message) { }
    }
}
