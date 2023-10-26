using Darts.Domain.Abstracts;
using Darts.Domain.DomainObjects;

namespace Darts.Api.Data.Repositories
{
   public class LeagueRepository : GenericRepository<League>, ILeagueRepository
   {
      public LeagueRepository(DartsContext dartsContext)
         : base(dartsContext)
      {
      }
   }
}
