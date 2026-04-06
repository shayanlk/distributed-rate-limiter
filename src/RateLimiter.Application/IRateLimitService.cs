namespace RateLimiter.Application;

/// <summary>
/// Defines the contract for checking rate limits in the application layer.
/// </summary>
public interface IRateLimitService
{
    // WHY: The API layer needs a single entry point to request rate limit decisions.
    // WHAT: Evaluates the provided request and returns the rate limit decision.
    Task<CheckRateLimitResponse> CheckAsync(CheckRateLimitRequest request, CancellationToken cancellationToken);
}
