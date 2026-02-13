using Common.Shared.Exceptions;
using Common.Shared.Helpers;

namespace Eve.API.Middlewares
{
    /// <summary>
    /// A custom middleware for handling exceptions that occur during the processing of HTTP requests in an ASP.NET Core application.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SecurityException ex)
            {
                await WriteErrorResponse(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                await WriteErrorResponse(context, StatusCodes.Status401Unauthorized, ex.Message);
            }
            catch (ForbidenException ex)
            {
                await WriteErrorResponse(context, StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                await WriteErrorResponse(context, StatusCodes.Status404NotFound, ex.Message);
            }
            catch (EntityAlreadyExistsException ex)
            {
                await WriteErrorResponse(context, StatusCodes.Status409Conflict, ex.Message);
            }
            catch (AccountLockedException ex)
            {
                await WriteErrorResponse(context, StatusCodes.Status423Locked, ex.Message);
            }
            catch (ActionFailedException ex)
            {
                await WriteErrorResponse(context, StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                await WriteErrorResponse(context, StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private async Task WriteErrorResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;

            var correlationId = CorrelationIdAccessor.GetGuid(context);

            var errorResponse = new { error = message, correlationId };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
