namespace Laktaren.Application.Interfaces
{
    public interface ITeamRepository
    {
        public Task<List<Team>> GetAllTeamsAsync();
    }
}
