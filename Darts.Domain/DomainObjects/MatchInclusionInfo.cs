namespace Darts.Domain.DomainObjects
{
   public class MatchInclusionInfo
   {
      public bool Teams { get; set; }
      public bool Players { get; set; }

      public static MatchInclusionInfo Empty => new MatchInclusionInfo();
   }
}
