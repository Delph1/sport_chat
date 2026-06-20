using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Laktaren.Application.Interfaces;
using Laktaren.Domain.Entities;

namespace Laktaren.Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IActionResult> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return new CreatedAtActionResult(nameof(GetByIdAsync), "Users", new { id = user.Id }, user);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            User? user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}
