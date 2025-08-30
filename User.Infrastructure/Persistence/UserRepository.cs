using Microsoft.EntityFrameworkCore;
using User.Infrastructure.Interfaces;

namespace User.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
  private readonly AppDbContext _db;
  public UserRepository(AppDbContext db) => _db = db;

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
    var user = await _db.Users.FindAsync(id);
    if (user is not null)
    {
      _db.Users.Remove(user);
      await _db.SaveChangesAsync();
    }
  }

  public Task<Domain.Entities.User?> GetByIdAsync(int id)
      => _db.Users.FirstOrDefaultAsync(u => u.Id == id);

  public Task<List<Domain.Entities.User>> GetAllAsync()
      => _db.Users.ToListAsync();
}
