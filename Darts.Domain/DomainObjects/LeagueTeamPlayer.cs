namespace Darts.Domain.DomainObjects
{
   public class LeagueTeamPlayer
   {
      public long LeagueId { get; set; }
      public long TeamId { get; set; }
      public long? PlayerId { get; set; }

      public Team Team { get; set; }
      public Player Player { get; set; }
      public League League { get; set; }
   }
}
