using Laktaren.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Laktaren.Application.Interfaces
{
    public interface IReactionRepository
    {
        public Task<List<Reaction>> GetReactionsByPostIdAsync(Guid postId);
        public Task<bool> ToggleReactionAsync(Reaction reaction);
    }
}
