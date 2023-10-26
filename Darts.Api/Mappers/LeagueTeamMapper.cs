using Darts.Contract;

namespace Darts.Api.Mappers
{
    public class LeagueTeamMapper
    {
        public static LeagueTeam Map(Domain.DomainObjects.LeagueTeam leagueTeamPlayer)
        {
            var result = Mapper.From<Domain.DomainObjects.LeagueTeam, LeagueTeam>(leagueTeamPlayer);
            result.TeamName = leagueTeamPlayer.Team?.Name;
            result.LeagueName = leagueTeamPlayer.League?.Name;

            return result;
        }
    }
}
