using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace RecipeCosting.Api.Helpers;

/// <summary>
/// Answers anything that was not expected with a problem document, and writes the
/// exception to the log, where the detail belongs. The caller is told that the
/// request failed and nothing about how.
/// </summary>
public class UnhandledExceptionHandler : IExceptionHandler
{
    private readonly ILogger<UnhandledExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetails;

    public UnhandledExceptionHandler(
        ILogger<UnhandledExceptionHandler> logger,
        IProblemDetailsService problemDetails)
    {
        _logger = logger;
        _problemDetails = problemDetails;
    }

    /// <summary>
    /// Logs the failure and writes a 500 that reads like every other error the API returns.
    /// </summary>
    /// <param name="httpContext">The request that failed.</param>
    /// <param name="exception">What went wrong.</param>
    /// <param name="cancellationToken">Token cancelled when the caller gives up.</param>
    /// <returns>True, because the response has been written.</returns>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Unhandled exception answering {Method} {Path}",
            httpContext.Request.Method,
            httpContext.Request.Path);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return await _problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "The request could not be completed.",
                Detail = "Something failed on the server. The failure has been logged.",
            },
        });
    }
}
