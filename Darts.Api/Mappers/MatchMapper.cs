using Darts.Contract;

namespace Darts.Api.Mappers
{
    public static class MatchMapper
    {
        public static Match Map(Domain.DomainObjects.Match match)
        {
            return Mapper.From<Domain.DomainObjects.Match, Match>(match);
        }
    }
}
