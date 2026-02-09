using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Shared.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an attempt is made to create an entity that already exists.
    /// </summary>
    /// <remarks>This exception is typically used to indicate a conflict when adding or registering an entity
    /// with a unique identifier or key. Use this exception to signal that the operation cannot proceed because the
    /// entity is already present in the data store or system.</remarks>
    public class EntityAlreadyExistsException : Exception
    {
        public string EntityName { get; }
        public object? EntityKey { get; }

        public EntityAlreadyExistsException(string entityName, object entityKey)
            : base($"{entityName} with id '{entityKey}' already exists.")
        {
            EntityName = entityName;
            EntityKey = entityKey;
        }

        public EntityAlreadyExistsException(string entityName, object entityKey, string identificator)
            : base($"{entityName} with {identificator} '{entityKey}' already exists.")
        {
            EntityName = entityName;
            EntityKey = entityKey;
        }

        public EntityAlreadyExistsException(string entityName)
            : base($"{entityName} already exists.")
        {
            EntityName = entityName;
        }
    }
}
