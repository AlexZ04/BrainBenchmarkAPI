using BrainBenchmarkAPI.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BrainBenchmarkAPI.Infrastructure
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occured: {Message}", exception.Message);

            ProblemDetails problemDetails;

            if (exception is CredentialsException credExc)
            {
                problemDetails = new ProblemDetails
                {
                    Status = credExc.Code,
                    Title = credExc.Error,
                    Detail = exception.Message
                };
            }
            else if (exception is UserNotFoundException userNotFoundExc)
            {
                problemDetails = new ProblemDetails
                {
                    Status = userNotFoundExc.Code,
                    Title = userNotFoundExc.Error,
                    Detail = exception.Message
                };
            }
            else
            {
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server errorrrr"
                };
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
