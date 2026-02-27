using System.Net;
using System.Text.Json;

namespace HotelManagmentAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred.";

            if (exception is ArgumentException || exception.Message.Contains("not found"))
            {
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else if (exception.Message.Contains("already exists") ||
                     exception.Message.Contains("already booked") ||
                     exception.Message.Contains("already been"))
            {
                statusCode = HttpStatusCode.Conflict;
                message = exception.Message;
            }
            else if (exception.Message.Contains("Only confirmed") ||
                     exception.Message.Contains("Only checked-in"))
            {
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
            }

            var result = JsonSerializer.Serialize(new
            {
                error = message,
                statusCode = (int)statusCode
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(result);
        }
    }
}