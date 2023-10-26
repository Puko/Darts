using Darts.Contract;

namespace Darts.Api.Mappers
{
    public class PremierLeaguePlayerMapper
    {
        public static PremierLeaguePlayer Map(Domain.DomainObjects.PremierLeaguePlayer premierLeaguePlayer)
        {
            var result = Mapper.From<Domain.DomainObjects.PremierLeaguePlayer, PremierLeaguePlayer>(premierLeaguePlayer);
            result.LeagueName = premierLeaguePlayer.PremierLeague?.Name;
            result.PlayerName = premierLeaguePlayer.Player?.FullName;

            return result;
        }
    }
}
