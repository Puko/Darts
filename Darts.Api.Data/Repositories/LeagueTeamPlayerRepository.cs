using Darts.Domain.Abstracts;
using Darts.Domain.DomainObjects;

namespace Darts.Api.Data.Repositories
{
   public class LeagueTeamPlayerRepository : GenericRepository<LeagueTeamPlayer>, ILeagueTeamPlayerRepository
   {
      public LeagueTeamPlayerRepository(DartsContext dartsContext) : base(dartsContext)
      {
      }
   }
}
