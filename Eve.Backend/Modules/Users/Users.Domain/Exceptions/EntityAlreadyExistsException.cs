using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Domain.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public string EntityName { get; }
        public object EntityKey { get; }

        public EntityAlreadyExistsException(string entityName, object entityKey)
            : base($"{entityName} with key '{entityKey}' already exists.")
        {
            EntityName = entityName;
            EntityKey = entityKey;
        }
    }
}
