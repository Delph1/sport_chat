using Laktaren.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Laktaren.Application.Interfaces
{
    public interface IReactionRepository
    {
        public Task<ReactionsDto> GetReactionsByPostIdAsync(Guid postId);
        public Task<ReactionsDto> ToggleReactionAsync(Reaction reaction);
    }
}
