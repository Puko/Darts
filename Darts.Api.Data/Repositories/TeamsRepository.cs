using Darts.Domain.Abstracts;
using Darts.Domain.DomainObjects;

namespace Darts.Api.Data.Repositories
{
   public class TeamsRepository : GenericRepository<Team>, ITeamsRepository
   {
      public TeamsRepository(DartsContext dartsContext)
       : base(dartsContext)
      {

      }
   }
}
