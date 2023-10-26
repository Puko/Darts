using System.Collections.Generic;
using System.Linq;
using Darts.Domain.Abstracts;
using Darts.Domain.DomainObjects;

namespace Darts.Api.Data.Repositories
{
   public class StatsRepository : GenericRepository<Stats>, IStatsRepository
   {
      public StatsRepository(DartsContext dartsContext)
         : base(dartsContext)
      {
      }
   }
}
