using Darts.Domain.Abstracts;
using Darts.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace Darts.Api.Data.Repositories
{
    public class PremierLeagueMatchRepository : GenericRepository<PremierLeagueMatch>, IPremierLeagueMatchRepository
    {
        public PremierLeagueMatchRepository(DartsContext databaseContext) : base(databaseContext)
        {
        }
    }
}
