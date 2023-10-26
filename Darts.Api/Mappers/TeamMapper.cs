using Darts.Contract;

namespace Darts.Api.Mappers
{
    public static class TeamMapper
    {
        public static Team Map(Domain.DomainObjects.Team team)
        {
            var result = Mapper.From<Domain.DomainObjects.Team, Team>(team);
            return result;
        }

        public static Domain.DomainObjects.Team Map(Team team)
        {
            var result = Mapper.From<Team, Domain.DomainObjects.Team>(team);
            return result;
        }
    }
}
