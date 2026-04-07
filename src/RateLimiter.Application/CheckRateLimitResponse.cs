namespace RateLimiter.Application;

/// <summary>
/// Represents the outcome of a rate limit check.
/// </summary>
// WHY: Clients need a consistent response that explains allow/deny and retry timing.
// WHAT: Carries decision, remaining tokens, and when the bucket refills to full.
public sealed record CheckRateLimitResponse(bool Allowed, int RemainingTokens, DateTimeOffset ResetAt);
