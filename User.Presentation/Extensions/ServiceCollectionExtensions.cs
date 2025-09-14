using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using User.Application.Common.Helpers;
using User.Application.Common.Interfaces;
using User.Infrastructure.Helpers;
using User.Infrastructure.Persistence;
using User.Infrastructure.Persistence.Audit;
using User.Infrastructure.Persistence.Entities;
using User.Infrastructure.Persistence.Services;

namespace User.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
  {
    var connString = config.GetConnectionString("PostDb");

    if (env.IsEnvironment("Testing"))
    {
      services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("TestDb"));
    }
    else
    {
      services.AddDbContext<AppDbContext>(opt =>
          opt.UseSqlServer(connString, sql => sql.MigrationsAssembly("User.Infrastructure")));

      services.AddDbContext<IdentityDbContext>(opt =>
          opt.UseSqlServer(connString, sql => sql.MigrationsAssembly("User.Infrastructure")));

      services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddEntityFrameworkStores<IdentityDbContext>()
          .AddDefaultTokenProviders();

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
            options.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer = config["Jwt:Issuer"],
              ValidAudience = config["Jwt:Audience"],
              IssuerSigningKey = new SymmetricSecurityKey(
                          Encoding.UTF8.GetBytes(config["Jwt:Secret"]!))
            };
          });
    }

    return services;
  }

  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
  {
    services.AddHttpClient<ITurnstileValidatorService, TurnstileValidatorService>();

    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IAuditLogger, AuditLogger>();
    services.AddScoped<IAuditBuilder, AuditBuilder>();
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IAppSettingsService, AppSettingsService>();
   // services.AddScoped<ITurnstileValidatorService, TurnstileValidatorService>();
    
    services.AddTransient<IEmailSender, SmtpEmailSender>();
    
    services.Configure<EmailConfiguration>(config.GetSection("EmailConfiguration"));
    services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(24));
    
    
    return services;
  }


  public static IServiceCollection AddCorsPolicy(this IServiceCollection services, string my)
  {
    services.AddCors(options =>
    {
      options.AddPolicy(/*"AllowFrontend"*/my, policy =>
      {
        policy.WithOrigins("https://localhost:7220",
    "http://localhost:7220",
    "https://127.0.0.1:7220")
              .AllowAnyHeader()
              .AllowAnyMethod().AllowCredentials(); ;
      });
      //options.AddPolicy("AllowLocalFile", policy =>
      //{
      //  policy.SetIsOriginAllowed(origin => origin == "null")
      //        .AllowAnyHeader()
      //        .AllowAnyMethod();
      //});
    });

    return services;
  }
}
