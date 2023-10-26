using Darts.Domain.Abstracts;
using Darts.Domain.DomainObjects;

namespace Darts.Api.Data.Repositories
{
   public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
   {
      public PlayerRepository(DartsContext dartsContext)
         : base(dartsContext)
      {

      }
   }
}
