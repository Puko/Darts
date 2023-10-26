using Darts.Domain.Abstracts;
using Darts.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace Darts.Api.Data.Repositories
{
    public class PremierLeaguePlayerRepository : GenericRepository<PremierLeaguePlayer>, IPremierLeaguePlayerRepository
    {
        public PremierLeaguePlayerRepository(DartsContext databaseContext) : base(databaseContext)
        {
        }
    }
}
