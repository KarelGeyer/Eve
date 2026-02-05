using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Shared.Exceptions
{
    /// <summary>
    /// Thrown in case an entity was not found in the data store.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public string EntityName { get; }

        public EntityNotFoundException(string message)
            : base(message)
        {
            EntityName = "Entity";
        }

        public EntityNotFoundException(string entityName, string message)
            : base(message)
        {
            EntityName = entityName;
        }
    }
}
