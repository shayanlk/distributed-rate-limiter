namespace RateLimiter.Domain;

/// <summary>
/// Implements the token bucket algorithm for rate limiting.
/// </summary>
public sealed class TokenBucket
{
    private readonly int _capacity;
    private readonly double _refillRatePerSecond;
    private readonly object _lock = new();
    private double _tokens;
    private DateTimeOffset _lastRefill;

    // WHY: A token bucket must start with a known capacity and refill behavior.
    // WHAT: Initializes the bucket with the configured policy and a full token count.
    public TokenBucket(RateLimitPolicy policy)
    {
        _capacity = policy.Capacity;
        _refillRatePerSecond = policy.RefillRatePerSecond;
        _tokens = policy.Capacity;
        _lastRefill = DateTimeOffset.UtcNow;
    }

    // WHY: The caller needs a single atomic operation to check and consume tokens safely.
    // WHAT: Refills tokens based on elapsed time, then consumes if enough are available.
    public bool TryConsume(out int remainingTokens, out DateTimeOffset resetAt, int tokensNeeded = 1)
    {
        lock (_lock)
        {
            var now = DateTimeOffset.UtcNow;
            Refill(now);

            var allowed = _tokens >= tokensNeeded;
            if (allowed)
            {
                _tokens -= tokensNeeded;
            }

            remainingTokens = (int)Math.Floor(_tokens);
            resetAt = CalculateFullResetAt(now);
            return allowed;
        }
    }

    // WHY: The bucket must replenish tokens based on real elapsed time for accuracy.
    // WHAT: Adds tokens proportional to elapsed seconds without exceeding capacity.
    private void Refill(DateTimeOffset now)
    {
        var elapsedSeconds = (now - _lastRefill).TotalSeconds;
        if (elapsedSeconds <= 0 || _refillRatePerSecond <= 0)
        {
            return;
        }

        var tokensToAdd = elapsedSeconds * _refillRatePerSecond;
        _tokens = Math.Min(_capacity, _tokens + tokensToAdd);
        _lastRefill = now;
    }

    // WHY: Clients want to know when their bucket will be fully refilled.
    // WHAT: Calculates the timestamp when the bucket reaches full capacity.
    private DateTimeOffset CalculateFullResetAt(DateTimeOffset now)
    {
        if (_tokens >= _capacity || _refillRatePerSecond <= 0)
        {
            return now;
        }

        var secondsToFull = (_capacity - _tokens) / _refillRatePerSecond;
        return now.AddSeconds(secondsToFull);
    }
}
