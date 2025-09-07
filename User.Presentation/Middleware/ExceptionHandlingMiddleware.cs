using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

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
    catch (ValidationException fvex)
    {
      _logger.LogWarning(fvex, "Błąd walidacji");

      var errors = fvex.Errors
     .GroupBy(e => e.PropertyName)
     .ToDictionary(
         g => g.Key,
         g => g.Select(e => e.ErrorMessage).ToArray()
     );

      var problemDetails = new ValidationProblemDetails(errors)
      {
        Type = "https://tools.ietf.org/html/rfc7807",
        Title = "Błąd walidacji",
        Status = StatusCodes.Status400BadRequest,
        Detail = "Wystąpiły błędy walidacyjne w przesłanym żądaniu.",
        Instance = context.Request.Path
      };

      context.Response.StatusCode = 400;
      context.Response.ContentType = "application/problem+json";

      var json = JsonSerializer.Serialize(problemDetails);

      await context.Response.WriteAsync(json);
    }
    catch (UnauthorizedAccessException ex)
    {
      var problem = new ProblemDetails
      {
        Type = "https://example.com/unauthorized",
        Title = "Brak autoryzacji",
        Status = StatusCodes.Status401Unauthorized,
        Detail = ex.Message,
        Instance = context.Request.Path
      };

      context.Response.StatusCode = 401;
      context.Response.ContentType = "application/problem+json";

      var json = JsonSerializer.Serialize(problem);

      await context.Response.WriteAsync(json);

    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Nieobsłużony wyjątek");
      context.Response.StatusCode = 500;
      await context.Response.WriteAsJsonAsync(new { Error = "Wewnętrzny błąd" });
    }
  }
}
