namespace User.Application.Common.Interfaces;

public interface IUserRepository
{
  Task AddAsync(Domain.Entities.User user);
  Task UpdateAsync(Domain.Entities.User user);
  Task DeleteAsync(int id);
  Task<Domain.Entities.User?> GetByIdAsync(int id);
  Task<List<Domain.Entities.User>> GetAllAsync();

  Task<bool> ExistsByEmailAsync(string email);
}