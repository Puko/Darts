using System;
using System.Linq;
using Darts.Domain.Abstracts;
using Darts.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace Darts.Api.Data.Repositories
{
   public class LeagueTeamRepository : GenericRepository<LeagueTeam>, ILeagueTeamRepository
   {
      public LeagueTeamRepository(DartsContext dartsContext) : base(dartsContext)
      {
      }

   }
}
