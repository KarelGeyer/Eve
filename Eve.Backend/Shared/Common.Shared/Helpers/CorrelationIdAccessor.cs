using Microsoft.AspNetCore.Http;

namespace Common.Shared.Helpers
{
    /// <summary>
    /// A helper class for accessing and managing correlation IDs in HTTP requests.
    /// This class provides constants for the header name and context item name used to store the correlation ID,
    /// as well as a method to retrieve the correlation ID as a GUID from the HTTP context.
    /// If the correlation ID is not present or cannot be parsed, a new GUID will be generated and returned.
    /// </summary>
    public static class CorrelationIdAccessor
    {
        public const string HeaderName = "X-Correlation-ID";
        public const string ContextItemName = "CorrelationId";

        /// <summary>
        /// A method to retrieve the correlation ID as a GUID from the HTTP context.
        /// It checks if the correlation ID is present in the context items and attempts to parse it as a GUID.
        /// If parsing fails or the correlation ID is not present, it generates and returns a new GUID.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>A <see cref="Guid"/> Corrlation ID bound to current request</returns>
        public static Guid GetGuid(HttpContext? context)
        {
            if (context != null && context.Items.TryGetValue(ContextItemName, out var id) && id is string stringId)
            {
                return Guid.TryParse(stringId, out var guid) ? guid : Guid.NewGuid();
            }

            return Guid.NewGuid();
        }
    }
}
