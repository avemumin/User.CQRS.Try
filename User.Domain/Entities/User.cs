namespace User.Domain.Entities;

public class User
{
  public int Id { get; private set; }
  public string Name { get; private set; } = "";
  public string LastName { get; private set; } = "";
  public string Email { get; private set; } = "";
  public int Age { get; private set; }

  public string IdentityUserId { get; set; }
 

  public User(string name, string lastName, string email, int age)
  {
    Name = name;
    LastName = lastName;
    Email = email;
    Age = age;
  }

  public void Update(string name, string lastName, string email, int age)
  {
    Name = name;
    LastName = lastName;
    Email = email;
    Age = age;
  }
}
