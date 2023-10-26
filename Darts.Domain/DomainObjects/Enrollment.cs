using System;

namespace Darts.Domain.DomainObjects
{
   public class Enrollment
   {
      public long Id { get; set; }
      public long? MatchId { get; set; }
      public long? PremierLeagueMatchId { get; set; }
      public byte[] Data { get; set; }
      public DateTimeOffset Uploaded { get; set; }
   }
}
