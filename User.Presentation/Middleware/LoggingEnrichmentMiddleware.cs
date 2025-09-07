using System.Diagnostics;
using Serilog.Context;

namespace User.Presentation.Middleware;

public class LoggingEnrichmentMiddleware
{
  private readonly RequestDelegate _next;

  public LoggingEnrichmentMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task Invoke(HttpContext context)
  {
    var traceId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString();
    var user = context.User.Identity?.Name ?? "anonymous";

    LogContext.PushProperty("TraceId", traceId);
    LogContext.PushProperty("User", user);
    LogContext.PushProperty("RequestPath", context.Request.Path);

    await _next(context);
  }
}
