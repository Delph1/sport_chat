using Laktaren.Domain.Entities;

namespace Laktaren.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}
