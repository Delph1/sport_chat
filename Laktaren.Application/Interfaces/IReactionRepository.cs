using Laktaren.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Laktaren.Domain.Contracts;

namespace Laktaren.Application.Interfaces
{
    public interface IReactionRepository
    {
        public Task<ReactionsDto> GetReactionsByPostIdAsync(Guid postId, Guid currentUserId);
        public Task<ReactionsDto> ToggleReactionAsync(Reaction reaction);
    }
}
