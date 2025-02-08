using BrainBenchmarkAPI.Constants;
using BrainBenchmarkAPI.Exceptions;
using BrainBenchmarkAPI.Models;
using System.Net;
using System.Text.Json;

namespace BrainBenchmarkAPI.Middlewares
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

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (CredentialsException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    HttpStatusCode.BadRequest);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, string exMsg, HttpStatusCode httpStatusCode)
        {
            _logger.LogError(exMsg);

            HttpResponse response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;

            ErrorModel errorModel = new ErrorModel()
            {
                Message = exMsg,
                StatusCode = (int)httpStatusCode
            };

            await response.WriteAsJsonAsync(errorModel);
        }
    }
}
