namespace Laktaren.Domain.Contracts
{
    public class ReactionsDto
    {
        public int LikeCount { get; set; }
        public int BooCount { get; set; }
        public Dictionary<string, int> LikesPerTeam { get; set; } = new();
        public Dictionary<string, int> BoosPerTeam { get; set; } = new();
        public ReactionType? UserReaction { get; set; } 
    }
}