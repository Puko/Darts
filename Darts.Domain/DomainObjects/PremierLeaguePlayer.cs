namespace Darts.Domain.DomainObjects
{
   public class PremierLeaguePlayer
   {
      public long PlayerId { get; set; }
      public long PremierLeagueId { get; set; }
      public Player Player { get; set; }
      public PremierLeague PremierLeague { get; set; }
   }
}
