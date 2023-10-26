using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Darts.Domain.DomainObjects
{
   public class League
   {
      public long Id { get; set; }
      [Required]
      public string Name { get; set; }
      [Required]
      public string ShortCut { get; set; }
      public string Note { get; set; }
      public byte[] Image { get; set; }
      public DateTimeOffset Created { get; set; }
      [Required]
      public int PointsPerWin { get; set; } = 3;
      [Required]
      public int PointsPerLoose { get; set; } = 0;
      [Required]
      public int PointsPerOvertimeWin { get; set; } = 2;
      [Required]
      public int PointsPerOvertimeLoose { get; set; } = 1;
      [Required]
      public int PointsForDraw { get; set; } = 1;
      [Required]
      public long UserId { get; set; }

      public User User { get; set; }

      public ICollection<LeagueTeam> LeagueTeams { get; set; } = new List<LeagueTeam>();
      public ICollection<LeagueTeamPlayer> LeagueTeamPlayers { get; set; } = new List<LeagueTeamPlayer>();
      public ICollection<Match> Matches { get; set; } = new List<Match>();
      public ICollection<Stats> Stats { get; set; } = new List<Stats>();

      public override bool Equals(object obj)
      {
         return obj is League league &&
                Id == league.Id;
      }

      public override int GetHashCode()
      {
         return HashCode.Combine(Id);
      }

      public static League Copy(League league)
      {
         var newLeague = new League
         {
            Created = DateTimeOffset.UtcNow,
            Name = $"Copy {league.Name}",
            Note = league.Note,
            ShortCut = league.ShortCut,
            UserId = league.UserId,
            PointsForDraw = league.PointsForDraw,
            PointsPerLoose = league.PointsPerLoose,
            PointsPerOvertimeLoose = league.PointsPerOvertimeLoose,
            PointsPerOvertimeWin = league.PointsPerOvertimeWin,
            PointsPerWin = league.PointsPerWin,
         };

         return newLeague;
      }
   }
}
