namespace User.Application.Commands;

public record CreateUser(string Name, string LastName, string Email, int Age);