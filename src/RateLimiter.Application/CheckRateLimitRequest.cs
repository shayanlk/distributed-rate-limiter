namespace RateLimiter.Application;

/// <summary>
/// Represents the data needed to request a rate limit check from the application layer.
/// </summary>
// WHY: The API layer needs a stable contract for rate limit check inputs.
// WHAT: Carries the API key and resource identifier to the application layer.
public sealed record CheckRateLimitRequest(string ApiKey, string Resource);
