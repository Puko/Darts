using Darts.Domain.Abstracts;
using Darts.Domain.DomainObjects;

namespace Darts.Api.Data.Repositories
{
   public class MatchRepository : GenericRepository<Match>, IMatchRepository
   {
      public MatchRepository(DartsContext dartsContext)
         : base(dartsContext)
      {
      }
   }
}
