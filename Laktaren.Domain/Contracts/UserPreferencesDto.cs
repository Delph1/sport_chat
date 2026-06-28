using System;
using System.Collections.Generic;
using System.Text;

namespace Laktaren.Domain.Contracts
{
    public class UserPreferencesDto
    {        
        public Guid TeamId {  get; set; }
        public bool UseTeamColors { get; set; }
        public required List<Guid?> SecondaryTeams {  get; set; }
    }
}
