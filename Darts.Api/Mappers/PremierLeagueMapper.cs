using Darts.Contract;

namespace Darts.Api.Mappers
{
    public static class PremierLeagueMapper
    {
        public static PremierLeague Map(Domain.DomainObjects.PremierLeague league)
        {
            var result = Mapper.From<Domain.DomainObjects.PremierLeague, PremierLeague>(league);
            result.PlayersCount = league.PremierLeaguePlayers?.Count ?? 0;
            result.Owner = $"{league.User?.FirstName} {league.User?.LastName}";

            return result;
        }
    }
}
