using System.Net;
using System.Text.Json;

namespace Bank.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);

            }

        }

        private static Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;
            var message = exception.Message;
            if(message.Contains("not found"))
            {
                statusCode = (int)HttpStatusCode.NotFound;
            }
            else if(message.Contains("Insufficient funds") || message.Contains("must be greater than zero") || message.Contains("must be different")
            ||message.Contains("Invalid email or password"))
            {
                statusCode = (int)HttpStatusCode.BadRequest;
            }
            else if(message.Contains("already in use"))
            {
                statusCode = (int)HttpStatusCode.Conflict;
            }

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            var response = new
            {
                StatusCode = statusCode,
                Message = message
            };

            var json = JsonSerializer.Serialize(response);
            return  httpContext.Response.WriteAsync(json);
        }
    }

}