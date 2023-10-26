using Darts.Contract;

namespace Darts.Api.Mappers
{
    public static class LeagueTeamPlayerMapper
    {
        public static LeagueTeamPlayer Map(Domain.DomainObjects.LeagueTeamPlayer leagueTeamPlayer)
        {
            var result = Mapper.From<Domain.DomainObjects.LeagueTeamPlayer, LeagueTeamPlayer>(leagueTeamPlayer);
            result.TeamName = leagueTeamPlayer.Team?.Name;
            result.LeagueName = leagueTeamPlayer.League?.Name;
            result.PlayerName = leagueTeamPlayer.Player?.FullName;

            return result;
        }
    }
}
