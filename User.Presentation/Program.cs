using Serilog;
using User.Application.Handlers;
using User.Infrastructure.Helpers;
using User.Presentation.Extensions;
using User.Presentation.Middleware;
using Wolverine;
using Wolverine.FluentValidation;

public partial class Program
{
  public static async Task Main(string[] args)
  {

    var builder = WebApplication.CreateBuilder(args);

    // Logging
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    builder.Host.UseSerilog();

    // Services
    builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddCorsPolicy();
    builder.Services.AddControllers();

    // Wolverine
    builder.Host.UseWolverine(options =>
    {
      options.Discovery.IncludeAssembly(typeof(CreateUserHandler).Assembly);
      options.Discovery.IncludeAssembly(typeof(DeleteUserHandler).Assembly);
      options.Discovery.IncludeAssembly(typeof(GetAllUsersHandler).Assembly);
      options.Discovery.IncludeAssembly(typeof(GetUserByIdHandler).Assembly);
      options.Discovery.IncludeAssembly(typeof(UpdateUserHandler).Assembly);
      options.UseFluentValidation();
    });

    var app = builder.Build();

    // Seed
    using (var scope = app.Services.CreateScope())
    {
      var service = scope.ServiceProvider;
      await SeedRoles.SeedRolesAsync(service);
    }

    // Middleware
    app.UseMiddleware<LoggingEnrichmentMiddleware>();
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors("AllowFrontend");
    //app.UseCors("AllowLocalFile");
    app.MapControllers();

    app.Run();

  }
}
