namespace RateLimiter.Application;

/// <summary>
/// Provides a stubbed rate limit service that always allows requests for Day 1 scaffolding.
/// </summary>
public sealed class RateLimitService : IRateLimitService
{
    // WHY: Day 1 focuses on proving the API plumbing without enforcing limits yet.
    // WHAT: Returns an allowed response with placeholder token and reset values.
    public Task<CheckRateLimitResponse> CheckAsync(
        CheckRateLimitRequest request,
        CancellationToken cancellationToken)
    {
        var response = new CheckRateLimitResponse(
            Allowed: true,
            RemainingTokens: 100,
            ResetAt: DateTimeOffset.UtcNow.AddMinutes(1));

        return Task.FromResult(response);
    }
}
