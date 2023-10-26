using Darts.Contract;

namespace Darts.Api.Mappers
{
    public static class PlayerMapper
    {
        public static Player Map(Domain.DomainObjects.Player player)
        {
            var result = Mapper.From<Domain.DomainObjects.Player, Player>(player);
            result.CreatedBy = $"{player.User?.FirstName} {player.User?.LastName}";
            return result;
        }
    }
}
