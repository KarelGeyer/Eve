using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Shared.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a specific action fails to complete successfully.
    /// </summary>
    /// <remarks>Use this exception to provide detailed information about failed actions, including the name
    /// of the action and an optional inner exception for additional context. This exception is typically thrown by
    /// methods that perform actions which may fail due to application-specific conditions.</remarks>
    public class ActionFailedException : Exception
    {
        public string ActionName { get; }

        public ActionFailedException(string actionName, string message)
            : base(message)
        {
            ActionName = actionName;
        }

        public ActionFailedException(string actionName, string message, Exception innerException)
            : base(message, innerException)
        {
            ActionName = actionName;
        }
    }
}
