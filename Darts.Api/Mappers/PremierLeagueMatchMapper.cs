using Darts.Contract;

namespace Darts.Api.Mappers
{
    public static class PremierLeagueMatchMapper
    {
        public static PremierLeagueMatch Map(Domain.DomainObjects.PremierLeagueMatch match)
        {
            var contract = Mapper.From<Domain.DomainObjects.PremierLeagueMatch, PremierLeagueMatch>(match);
            contract.HomePlayerName = match.HomePlayer?.FullName;
            contract.GuestPlayerName = match.GuestPlayer?.FullName;

            return contract;
        }
    }
}
