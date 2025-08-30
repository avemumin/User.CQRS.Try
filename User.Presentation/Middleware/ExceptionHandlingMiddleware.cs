namespace User.Presentation.Middleware;

public class ExceptionHandlingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ExceptionHandlingMiddleware> _logger;

  public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task Invoke(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (FluentValidation.ValidationException fvex)
    {
      _logger.LogWarning(fvex, "Błąd walidacji");
      context.Response.StatusCode = 400;
      context.Response.ContentType = "application/json";

      var errors = fvex.Errors.Select(e => new
      {
        Field = e.PropertyName,
        Message = e.ErrorMessage,
        Code = e.ErrorCode
      });

      await context.Response.WriteAsJsonAsync(new { Errors = errors });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Nieobsłużony wyjątek");
      context.Response.StatusCode = 500;
      await context.Response.WriteAsJsonAsync(new { Error = "Wewnętrzny błąd" });
    }
  }
}
