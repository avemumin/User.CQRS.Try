using Microsoft.EntityFrameworkCore;
using User.Application.Handlers;
using User.Infrastructure.Interfaces;
using User.Infrastructure.Persistence;
using User.Presentation.Middleware;
using Wolverine;
using Wolverine.FluentValidation;

public partial class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);


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
    }



    builder.Services.AddScoped<IUserRepository, UserRepository>();
    // Add services to the container.

    builder.Services.AddControllers();

    //builder.Services.AddValidatorsFromAssembly(typeof(CreateUserValidator).Assembly);

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
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

  }
}
