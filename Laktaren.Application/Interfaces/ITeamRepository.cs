namespace Laktaren.Application.Interfaces
{
    public interface ITeamRepository
    {
        public Task<List<Team>> GetAllTeamsAsync();
        public Task FollowTeamAsync(Guid userId, Guid teamId);
    }
}
