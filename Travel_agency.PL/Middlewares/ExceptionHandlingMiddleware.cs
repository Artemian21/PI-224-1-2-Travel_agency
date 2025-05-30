using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Travel_agency.Core.Exceptions;

namespace Travel_agency.PL.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, "Exception caught in middleware");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            string errorType;

            switch (exception)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorType = "Not Found";
                    break;
                case ConflictException:
                    statusCode = HttpStatusCode.Conflict;
                    errorType = "Conflict";
                    break;
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    errorType = "Unauthorized";
                    break;
                case ValidationException:
                case BusinessValidationException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorType = "Validation Error";
                    break;
                case ArgumentNullException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorType = "Bad Request";
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    errorType = "Internal Server Error";
                    break;
            }

            var result = JsonSerializer.Serialize(new
            {
                error = errorType,
                statusCode = (int)statusCode,
                message = exception.Message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}
