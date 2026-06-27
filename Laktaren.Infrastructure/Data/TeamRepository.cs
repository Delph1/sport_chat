using Laktaren.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Laktaren.Domain.Entities;

namespace Laktaren.Infrastructure.Data
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;
        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams
                            .OrderBy(t => t.Name)
                            .ToListAsync();
        }

        public async Task FollowTeamAsync(Guid userId, Guid teamId)
        {
            var user = await _context.Users.Include(u => u.SecondaryTeams).FirstOrDefaultAsync(u => u.Id == userId);
            var team = await _context.Teams.FindAsync(teamId);

            if (user != null && team != null && !user.SecondaryTeams.Contains(team))
            {
                user.SecondaryTeams.Add(team);
                await _context.SaveChangesAsync();
            }
        }
    }
}
