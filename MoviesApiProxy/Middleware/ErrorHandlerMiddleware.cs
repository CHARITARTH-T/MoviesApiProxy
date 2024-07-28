using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MoviesApiProxy.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
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
            context.Response.ContentType = "text/plain";  // Change content type to plain text
            HttpStatusCode statusCode;
            string message;

            switch (exception)
            {
                case MovieNotFoundException _:
                    statusCode = HttpStatusCode.NotFound;
                    message = "Movie not found.";
                    break;
                case InvalidMovieIdException _:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Invalid or missing movie ID.";
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred. Please try again later.";
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(message);
        }
    }

    public class MovieNotFoundException : Exception
    {
        public MovieNotFoundException(string message) : base(message) { }
    }

    public class InvalidMovieIdException : Exception
    {
        public InvalidMovieIdException(string message) : base(message) { }
    }
}
