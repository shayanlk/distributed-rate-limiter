namespace RateLimiter.Domain;

/// <summary>
/// Encapsulates the rules that define a rate limit policy for a client.
/// </summary>
public sealed record RateLimitPolicy(int Capacity, int TokensPerInterval, TimeSpan Interval)
{
    // WHY: The token bucket refills smoothly, so we need a per-second rate.
    // WHAT: Calculates how many tokens are added each second based on the policy interval.
    public double RefillRatePerSecond => TokensPerInterval / Interval.TotalSeconds;
}
