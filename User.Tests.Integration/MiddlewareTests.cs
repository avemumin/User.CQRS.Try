using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using User.Application.DTOs;
using User.Infrastructure.Persistence;

namespace User.Tests.Integration;

public class MiddlewareTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public MiddlewareTests(WebApplicationFactory<Program> factory)
  {
    var testFactory = factory.WithWebHostBuilder(builder =>
    {
      builder.UseEnvironment("Testing");
      builder.ConfigureServices(services =>
      {
        var descriptor = services.SingleOrDefault(
          d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
        if (descriptor != null)
          services.Remove(descriptor);

        services.AddDbContext<AppDbContext>(options =>
        {
          options.UseInMemoryDatabase("TestDb");
        });

        AddSomeDataToInMemoryDb(services);
      });
    });

    _client = testFactory.CreateClient();

  }

  [Fact]
  public async Task Should_Return_ProblemDetails_When_ValidationFails()
  {
    //Arrange
    var payload = new
    {
      Name = "Staś",
      LastName = "Szczepański",
      Email = "fistashek1@gmail.com",
      Age = 5
    };

    //Act
    var response = await _client.PostAsJsonAsync("api/users", payload);

    //Assert
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

    var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

    Assert.NotNull(problem);
    Assert.True(problem.Errors.ContainsKey("Email"));
    Assert.Contains("Email już istnieje w bazie", problem.Errors["Email"]);

  }

  [Fact]
  public async Task Should_Created_User_And_Return_CreatedAction()
  {
    //Arrange
    var payload = new
    {
      Name = "John",
      LastName = "Wick",
      Email = "johnwick@killer.com",
      Age = 40
    };

    //Act
    var response = await _client.PostAsJsonAsync("api/users", payload);

    //Assert
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    Assert.NotNull(response.Headers.Location);

    var user = await response.Content.ReadFromJsonAsync<UserDto>();

    Assert.NotNull(user);
    Assert.Equal("John", user.Name);
    Assert.Equal("johnwick@killer.com", user.Email);

  }

  /// <summary>
  /// Add initial data to DbInMemory
  /// </summary>
  /// <param name="services"></param>
  private void AddSomeDataToInMemoryDb(IServiceCollection services)
  {
    var sp = services.BuildServiceProvider();
    using var scope = sp.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Users.Add(new Domain.Entities.User("Staś","Szczepański","fistashek1@gmail.com",5));

    db.SaveChanges();
  }
}
