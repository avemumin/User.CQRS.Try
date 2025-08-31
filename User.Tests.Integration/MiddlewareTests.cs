using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using User.Application.DTOs;

namespace User.Tests.Integration;

public class MiddlewareTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public MiddlewareTests(WebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
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
    var payload = new
    {
      Name = "John",
      LastName = "Wick",
      Email = "johnwick@killer.com",
      Age = 40
    };

    var response = await _client.PostAsJsonAsync("api/users", payload);

    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    Assert.NotNull(response.Headers.Location);

    var user = await response.Content.ReadFromJsonAsync<UserDto>();
    Assert.NotNull(user);
    Assert.Equal("John", user.Name);
    Assert.Equal("johnwick@killer.com", user.Email);

  }
}
