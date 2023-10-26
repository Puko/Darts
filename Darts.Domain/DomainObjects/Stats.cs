using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Darts.Domain.DomainObjects
{
   public class Stats
   {
      public long Id { get; set; }
      [Required]
      public decimal Points { get; set; }
      [Required]
      public decimal Games { get; set; }
      public decimal? WinLegs { get; set; }
      public decimal? LooseLegs { get; set; }
      public long PlayerId { get; set; }
      public Player Player { get; set; }
      public long MatchId { get; set; }
      public Match Match { get; set; }

      [NotMapped]
      public bool IsEmpty => Points == 0 && Games == 0;

      public override bool Equals(object obj)
      {
         return obj is Stats stats &&
                PlayerId == stats.PlayerId &&
                MatchId == stats.MatchId;
      }

      public override int GetHashCode()
      {
         return HashCode.Combine(Id);
      }
   }
}
