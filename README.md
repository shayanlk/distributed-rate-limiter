# Distributed Rate Limiter Service

## What This Is
A microservice for enforcing API rate limits across distributed systems using token bucket algorithms.

## Day 1: Project Scaffold
- [x] Clean Architecture structure
- [x] Single endpoint responding
- [x] Domain models defined
- [x] Actual rate limiting (Day 2)

## Run It
```bash
cd src/RateLimiter.Api
dotnet run
curl -X POST http://localhost:5000/api/ratelimit/check \
  -H "Content-Type: application/json" \
  -d '{"apiKey":"test-key","resource":"/users"}'
```

Expected response: `{"allowed":true,"remainingTokens":9,"resetAt":"..."}`

## Day 2: Token Bucket Algorithm
- [x] Real rate limiting (10 requests/minute)
- [x] Returns 429 when limit exceeded
- [x] Retry-After header

### Test It:
```bash
# Should succeed 10 times, then fail with 429
for i in {1..15}; do
  curl -X POST http://localhost:5000/api/ratelimit/check \
    -H "Content-Type: application/json" \
    -d '{"apiKey":"test-key","resource":"/users"}' \
    -w "\nStatus: %{http_code}\n"
  sleep 0.5
done
```

Expected: First 10 return 200, next 5 return 429
