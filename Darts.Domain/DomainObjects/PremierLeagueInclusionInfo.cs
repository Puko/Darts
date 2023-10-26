namespace Darts.Domain.DomainObjects
{
   public class PremierLeagueInclusionInfo
   {
      public bool User { get; set; }
      public bool Players { get; set; }

      public static PremierLeagueInclusionInfo Empty = new PremierLeagueInclusionInfo();
   }
}
