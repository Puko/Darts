using Darts.Contract;

namespace Darts.Api.Mappers
{
    public static class LeagueMapper
    {
        public static League Map(Domain.DomainObjects.League league)
        {
            var result = Mapper.From<Domain.DomainObjects.League, League>(league);
            result.TeamsCount = league.LeagueTeams.Count;
            result.Owner = $"{league.User?.FirstName} {league.User?.LastName}";
            return result;
        }

        public static Domain.DomainObjects.League Map(League league)
        {
            var result = Mapper.From<League, Domain.DomainObjects.League>(league);
            return result;
        }
    }
}
