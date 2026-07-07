using Laktaren.Domain.Entities;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Collections.Specialized.BitVector32;

namespace Laktaren.Domain.Contracts
{
    public class PostDto
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
        public Post? ParentPost { get; set; }
        public ReactionsDto Reactions { get; set; } = new ReactionsDto();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}