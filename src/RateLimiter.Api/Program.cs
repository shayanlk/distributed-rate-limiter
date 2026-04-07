using System.Globalization;
using RateLimiter.Application;

var builder = WebApplication.CreateBuilder(args);

// WHY: We need to expose OpenAPI metadata so teams can validate the scaffolded endpoint quickly.
// WHAT: Adds Swagger services for minimal API endpoints.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// WHY: The API must resolve the rate limit service from DI to keep the API layer thin.
// WHAT: Registers the application-layer rate limit service implementation.
builder.Services.AddScoped<IRateLimitService, RateLimitService>();

var app = builder.Build();

// WHY: Swagger provides a quick, no-code way to validate the Day 1 scaffold.
// WHAT: Enables Swagger middleware and UI in development.
app.UseSwagger();
app.UseSwaggerUI();

// WHY: The team needs a clear signal that the service booted correctly for Day 2.
// WHAT: Logs a startup message indicating token bucket enforcement is active.
app.Logger.LogInformation("Rate Limiter API started - Day 2: Token bucket enabled");

// WHY: This endpoint is the entry point for all rate limit checks.
// Clients call this before making their actual API request.
// WHAT: Accepts API key + resource, returns whether the request is allowed.
app.MapPost("/api/ratelimit/check", RateLimitEndpoints.HandleCheckAsync)
.WithName("CheckRateLimit")
.WithOpenApi();

app.Run();

/// <summary>
/// Provides minimal API endpoint handlers for rate limit operations.
/// </summary>
public static class RateLimitEndpoints
{
    // WHY: The API needs a single handler method so logic stays testable and reusable.
    // WHAT: Delegates the request to the rate limit service and returns the response.
    public static async Task<IResult> HandleCheckAsync(
        CheckRateLimitRequest request,
        IRateLimitService rateLimitService,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var response = await rateLimitService.CheckAsync(request, cancellationToken);

        if (response.Allowed)
        {
            return Results.Ok(response);
        }

        // WHY: Clients need to know how long to wait before retrying.
        // WHAT: Computes seconds until full refill and sets Retry-After for 429 responses.
        var retryAfterSeconds = Math.Max(
            0,
            (int)Math.Ceiling((response.ResetAt - DateTimeOffset.UtcNow).TotalSeconds));

        httpContext.Response.Headers["Retry-After"] =
            retryAfterSeconds.ToString(CultureInfo.InvariantCulture);

        return Results.Json(response, statusCode: StatusCodes.Status429TooManyRequests);
    }
}
