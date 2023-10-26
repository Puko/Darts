namespace Darts.Domain.DomainObjects
{
   
   public class LeagueIncusionInfo
   {
      public bool Teams { get; set; }
      public bool User { get; set; }
      
      public static LeagueIncusionInfo Empty => new LeagueIncusionInfo();
   }
}
