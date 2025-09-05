using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using User.Application.Common.Helpers;
using User.Application.Common.Interfaces;
using User.Application.Handlers;
using User.Infrastructure.Persistence;
using User.Infrastructure.Persistence.Audit;
using User.Infrastructure.Persistence.Entities;
using User.Infrastructure.Persistence.Services;
using User.Presentation.Middleware;
using Wolverine;
using Wolverine.FluentValidation;

public partial class Program
{
  public static void Main(string[] args)
  {


    var builder = WebApplication.CreateBuilder(args);
    //Serilog configuration
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    builder.Host.UseSerilog();


    //If use Testing do not use production db.
    if (builder.Environment.IsEnvironment("Testing"))
    {
      builder.Services.AddDbContext<AppDbContext>(testOptions =>
      {
        testOptions.UseInMemoryDatabase("TestDb");
      });
    }
    else
    {
      var connString = builder.Configuration.GetConnectionString("PostDb");
      builder.Services.AddDbContext<AppDbContext>(opt =>
      {
        opt.UseSqlServer(connString, sql => sql.MigrationsAssembly("User.Infrastructure"));
      });

      builder.Services.AddDbContext<IdentityDbContext>(opt =>
      {
        opt.UseSqlServer(connString, sql => sql.MigrationsAssembly("User.Infrastructure"));
      });

      builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<IdentityDbContext>()
        .AddDefaultTokenProviders();

      builder.Services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
.AddJwtBearer(options =>
{
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,

    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
  };
});
    }


    //
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IAuditLogger, AuditLogger>();
    builder.Services.AddScoped<IAuditBuilder, AuditBuilder>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    // Add services to the container.

    builder.Services.AddControllers();

    //builder.Services.AddValidatorsFromAssembly(typeof(CreateUserValidator).Assembly);

    //Wolverine know where to check Handlers
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

    //Middlewares
    app.UseMiddleware<LoggingEnrichmentMiddleware>();
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

  }
}
