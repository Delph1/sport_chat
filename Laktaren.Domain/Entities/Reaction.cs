using Laktaren.Domain.Entities;

public class Reaction
{
    public int Id { get; set; }
    public bool IsLike { get; set; }
    public Guid PostId { get; set; }
    public Post? Post { get; set; } = null!;
    public Guid UserId { get; set; }
    public User? User { get; set; } = null!;
    public Guid? TeamId { get; set; }
    public Team? Team { get; set; } = null!;
}