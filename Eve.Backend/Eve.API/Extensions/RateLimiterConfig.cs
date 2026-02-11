using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace Eve.API.Extensions
{
    /// <summary>
    /// A static class that provides extension methods for configuring rate limiting in the application.
    /// It defines various rate limiting policies, including global limits and specific limits for different user groups (e.g., strict, free, extreme).
    /// The class also handles the response when a request is rejected due to exceeding the rate limit, returning a standardized JSON response with error details and retry information.
    /// </summary>
    public static class RateLimiterConfig
    {
        public static IServiceCollection AddAppRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                var strictSettings = configuration.GetSection("RateLimiting:Strict");
                var freeSettings = configuration.GetSection("RateLimiting:Free");
                var registration = configuration.GetSection("RateLimiting:Registration");

                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    // No limit for localhost/internal calls
                    if (context.Connection.RemoteIpAddress?.ToString() == "::1")
                        return RateLimitPartition.GetNoLimiter("internal");

                    return RateLimitPartition.GetFixedWindowLimiter(
                        "global",
                        _ => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 20,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0,
                        }
                    );
                });

                options.AddFixedWindowLimiter(
                    "strict",
                    opt =>
                    {
                        opt.PermitLimit = strictSettings.GetValue<int>("PermitLimit");
                        opt.Window = TimeSpan.FromMinutes(strictSettings.GetValue<int>("WindowMinutes"));
                        opt.QueueLimit = strictSettings.GetValue<int>("QueueLimit");
                    }
                );

                options.AddFixedWindowLimiter(
                    "free",
                    opt =>
                    {
                        opt.PermitLimit = freeSettings.GetValue<int>("PermitLimit");
                        opt.Window = TimeSpan.FromMinutes(freeSettings.GetValue<int>("WindowMinutes"));
                        opt.QueueLimit = freeSettings.GetValue<int>("QueueLimit");
                    }
                );

                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString();
                    }

                    await context.HttpContext.Response.WriteAsJsonAsync(
                        new
                        {
                            error = "Too many requests",
                            message = "Zpomalte, zkuste to prosím později.",
                            retryAfterSeconds = retryAfter.TotalSeconds,
                        },
                        token
                    );
                };
            });

            return services;
        }
    }
}
