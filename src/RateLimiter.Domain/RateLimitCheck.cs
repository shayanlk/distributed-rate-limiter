namespace RateLimiter.Domain;

/// <summary>
/// Represents a request to check if an API call is within rate limits.
/// </summary>
// WHY: The domain needs a canonical representation of a rate limit check event.
// WHAT: Captures API key, resource, and request timestamp for auditing and logic.
public sealed record RateLimitCheck(string ApiKey, string Resource, DateTimeOffset Timestamp);
