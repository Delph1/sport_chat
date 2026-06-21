using static System.Collections.Specialized.BitVector32;

namespace Laktaren.Domain.Entities
{
    public class Post
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public User? Author { get; set; }
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}