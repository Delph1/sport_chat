using Laktaren.Domain.Entities;

public class Reaction
{
    public int Id { get; set; }
    public bool IsLike { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;
}