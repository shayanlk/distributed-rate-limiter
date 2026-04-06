namespace RateLimiter.Application;

/// <summary>
/// Represents the outcome of a rate limit check.
/// </summary>
public sealed record CheckRateLimitResponse(bool Allowed, int RemainingTokens, DateTimeOffset ResetAt);
