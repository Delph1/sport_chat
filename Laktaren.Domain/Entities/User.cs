namespace Laktaren.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public required string Password { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
