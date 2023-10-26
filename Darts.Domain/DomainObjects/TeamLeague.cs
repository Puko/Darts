namespace Darts.Domain.DomainObjects
{
   public class LeagueTeam
   {
      public long TeamId { get; set; }
      public Team Team { get; set; }
      public long LeagueId { get; set; }
      public League League { get; set; }
   }
}
