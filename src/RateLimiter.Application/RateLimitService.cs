using System.Collections.Concurrent;
using RateLimiter.Domain;

namespace RateLimiter.Application;

/// <summary>
/// Provides an in-memory token bucket rate limiting service for Day 2.
/// </summary>
public sealed class RateLimitService : IRateLimitService
{
    // WHY: Multiple requests can arrive concurrently for the same or different clients.
    // WHAT: Stores token buckets per API key with thread-safe access.
    private readonly ConcurrentDictionary<string, TokenBucket> _buckets =
        new(StringComparer.Ordinal);

    // WHY: Day 2 uses a simple fixed limit until policies become configurable.
    // WHAT: Defines a 10 requests per minute default policy.
    private readonly RateLimitPolicy _defaultPolicy = new(
        Capacity: 10,
        TokensPerInterval: 10,
        Interval: TimeSpan.FromMinutes(1));

    // WHY: The API layer needs a single call to evaluate rate limits per request.
    // WHAT: Retrieves or creates a bucket, consumes a token, and returns the decision.
    public Task<CheckRateLimitResponse> CheckAsync(
        CheckRateLimitRequest request,
        CancellationToken cancellationToken)
    {
        var bucket = _buckets.GetOrAdd(
            request.ApiKey,
            _ => new TokenBucket(_defaultPolicy));

        var allowed = bucket.TryConsume(
            remainingTokens: out var remainingTokens,
            resetAt: out var resetAt,
            tokensNeeded: 1);

        var response = new CheckRateLimitResponse(
            Allowed: allowed,
            RemainingTokens: remainingTokens,
            ResetAt: resetAt);

        return Task.FromResult(response);
    }
}
