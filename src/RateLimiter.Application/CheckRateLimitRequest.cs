namespace RateLimiter.Application;

/// <summary>
/// Represents the data needed to request a rate limit check from the application layer.
/// </summary>
public sealed record CheckRateLimitRequest(string ApiKey, string Resource);
