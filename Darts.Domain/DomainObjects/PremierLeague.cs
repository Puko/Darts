using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Darts.Domain.DomainObjects
{
   public class PremierLeague
   {
      public long Id { get; set; }
      [Required]
      public string Name { get; set; }
      [Required]
      public string ShortCut { get; set; }
      [Required]
      public int PointsPerWin { get; set; } = 3;
      [Required]
      public int PointsPerLoose { get; set; } = 0;
      [Required]
      public int PointsForDraw { get; set; } = 1;
      public DateTimeOffset Created { get; set; }

      public ICollection<PremierLeaguePlayer> PremierLeaguePlayers { get; set; }
      public ICollection<PremierLeagueMatch> PremierLeagueMatches { get; set; }
      public long UserId { get; set; }
      public User User { get; set; }
   }
}
