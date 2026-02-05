using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Domain.Exceptions
{
    /// <summary>
    /// Výjimka vyhazovaná v případě, že doménová operace nemohla být dokončena nebo selhala.
    /// </summary>
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
