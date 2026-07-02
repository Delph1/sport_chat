using System.Text.Json.Serialization;
using static System.Collections.Specialized.BitVector32;

namespace Laktaren.Domain.Entities
{
    public class Post
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public User? Author { get; set; }
        public int ReplyCount { get; set; }
        public Guid? ParentPostId { get; set; }
        public bool IsClubHouseOnly { get; set; } = false;
        public Guid? TargetTeamId { get; set; }
        [JsonIgnore]
        public Post? ParentPost { get; set; }
        [JsonIgnore]
        public ICollection<Post> Replies { get; set; } = new List<Post>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}