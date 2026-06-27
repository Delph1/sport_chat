using System.Text.Json.Serialization;

namespace Laktaren.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public required string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? TeamId { get; set; } //Favorite team
        public bool UseTeamColors { get; set; }
        [JsonIgnore]
        public ICollection<Team> SecondaryTeams { get; set; } = new List<Team>();
        [JsonIgnore]
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}