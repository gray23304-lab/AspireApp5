using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace ParcelTrack.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString("N")[..8];
        context.Items["RequestId"] = requestId;
        context.Response.Headers["X-Request-Id"] = requestId;

        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var method = context.Request.Method;
            var path = context.Request.Path.Value ?? "/";
            var statusCode = context.Response.StatusCode;
            var elapsedMs = stopwatch.ElapsedMilliseconds;

            Console.WriteLine($"[{requestId}] {method} {path} -> {statusCode} in {elapsedMs} ms");
        }
    }
}