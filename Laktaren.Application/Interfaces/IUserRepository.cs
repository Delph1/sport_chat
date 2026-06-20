using Laktaren.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Laktaren.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task <IActionResult> CreateUserAsync(User user);
        Task <User> UpdateUserAsync(User user);
        Task <IActionResult> DeleteUserAsync(Guid id);
    }
}
