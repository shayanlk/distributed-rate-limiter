namespace RateLimiter.Domain;

/// <summary>
/// Represents a request to check if an API call is within rate limits.
/// </summary>
public sealed record RateLimitCheck(string ApiKey, string Resource, DateTimeOffset Timestamp);
