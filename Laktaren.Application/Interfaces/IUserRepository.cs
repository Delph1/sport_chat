using Laktaren.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Laktaren.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetMeAsync(Guid userId);
        Task<User?> GetByIdAsync(Guid id);
        Task <User> CreateUserAsync(User user);
        Task <User> UpdateUserAsync(User user);
        Task <bool> DeleteUserAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> SavePreferencesAsync(User user);
    }
}
