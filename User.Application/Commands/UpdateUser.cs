namespace User.Application.Commands;

public record UpdateUser(int Id, string Name, string LastName, string Email, int Age);
