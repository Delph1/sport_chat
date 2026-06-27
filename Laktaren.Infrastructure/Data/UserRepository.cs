using Microsoft.EntityFrameworkCore;
using Laktaren.Application.Interfaces;
using Laktaren.Domain.Entities;
// Vi tar bort "using Microsoft.AspNetCore.Mvc;" eftersom det bara hör hemma i API:et!

namespace Laktaren.Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetMeAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            User? user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true; // Lyckad radering
            }

            return false; // Användaren fanns inte
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> SavePreferencesAsync(User updatedUser)
        {
            var existingUser = await _context.Users.FindAsync(updatedUser.Id);

            if (existingUser == null)
            {
                return false;
            }

            existingUser.TeamId = updatedUser.TeamId;
            existingUser.UseTeamColors = updatedUser.UseTeamColors;
            existingUser.SecondaryTeams = updatedUser.SecondaryTeams;

            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0;
        }
    }
}