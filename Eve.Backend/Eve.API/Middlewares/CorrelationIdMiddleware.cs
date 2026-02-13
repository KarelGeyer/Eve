using Common.Shared.Helpers;

namespace Eve.API.Middlewares
{
    /// <summary>
    /// A middleware component that generates a unique correlation ID for each incoming HTTP request and adds it to the request context and response headers.
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationHeader = "X-Correlation-ID";

        public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

        /// <summary>
        /// A method that is called for each incoming HTTP request.
        /// It checks for an existing correlation ID in the request headers, generates a new one if not found, and adds it to the request context and response headers.
        /// It also ensures that the correlation ID is included in the Serilog logging context for consistent tracing across logs.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId =
                context.Request.Headers[CorrelationIdAccessor.HeaderName].FirstOrDefault() ?? Guid.NewGuid().ToString();

            context.Items[CorrelationIdAccessor.ContextItemName] = correlationId;
            context.Response.Headers.Append(CorrelationIdAccessor.HeaderName, correlationId);

            using (Serilog.Context.LogContext.PushProperty(CorrelationIdAccessor.ContextItemName, correlationId))
            {
                await _next(context);
            }
        }
    }
}
