# Distributed Rate Limiter Service

## What This Is
A microservice for enforcing API rate limits across distributed systems using token bucket algorithms.

## Day 1: Project Scaffold
- ✅ Clean Architecture structure
- ✅ Single endpoint responding
- ✅ Domain models defined
- ⏳ Actual rate limiting (coming Day 2)

## Run It
```bash
cd src/RateLimiter.Api
dotnet run
curl -X POST http://localhost:5000/api/ratelimit/check \
  -H "Content-Type: application/json" \
  -d '{"apiKey":"test-key","resource":"/users"}'
```

Expected response: `{"allowed":true,"remainingTokens":100,"resetAt":"..."}`
