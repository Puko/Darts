using Darts.Contract;

namespace Darts.Api.Mappers
{
    public static class StatsMapper
    {
        public static Stats Map(Domain.DomainObjects.Stats stats)
        {
            var result = Mapper.From<Domain.DomainObjects.Stats, Stats>(stats);
            result.PlayerName = stats.Player?.FullName;
            result.MatchFormatted = stats.Match?.Formatted;
            result.MatchResult = stats.Match?.Result;

            return result;
        }

        public static Domain.DomainObjects.Stats ToDomain(Stats stats)
        {
            return new Domain.DomainObjects.Stats
            {
                Id = stats.Id,
                Games = stats.Games,
                LooseLegs = stats.LooseLegs,
                MatchId = stats.MatchId,
                PlayerId = stats.PlayerId,
                Points = stats.Points,
                WinLegs = stats.WinLegs
            };
        }
    }
}
