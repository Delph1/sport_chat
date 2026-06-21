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
    }
}
