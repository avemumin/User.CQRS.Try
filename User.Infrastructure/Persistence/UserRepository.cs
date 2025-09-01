using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.Application.Common.Interfaces;


namespace User.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
  private readonly AppDbContext _db;
  private readonly ILogger<UserRepository> _logger;
  public UserRepository(AppDbContext db, ILogger<UserRepository> logger)
  {
    _db = db;
    _logger = logger;
  }

  public async Task AddAsync(Domain.Entities.User user)
  {
    await _db.Users.AddAsync(user);
    await _db.SaveChangesAsync();
  }

  public async Task UpdateAsync(Domain.Entities.User user)
  {
    _db.Users.Update(user);
    await _db.SaveChangesAsync();
  }

  public async Task DeleteAsync(int id)
  {
    _logger.LogDebug("Usuwam użytkownika o ID {UserUd}", id);
    var user = await _db.Users.FindAsync(id);
    if (user is not null)
    {
      _db.Users.Remove(user);
      await _db.SaveChangesAsync();
    }
  }

  public async Task<Domain.Entities.User?> GetByIdAsync(int id)
      => await _db.Users.FirstOrDefaultAsync(u => u.Id == id);

  public async Task<List<Domain.Entities.User>> GetAllAsync()
      => await _db.Users.ToListAsync();

  public async Task<bool> ExistsByEmailAsync(string email)
    => await _db.Users.AnyAsync(u => u.Email == email);
}
