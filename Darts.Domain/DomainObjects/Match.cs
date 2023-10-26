using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Darts.Domain.DomainObjects
{
    public class Match
    {
        public long Id { get; set; }
        public decimal? HomeLegs { get; set; }
        public decimal? HomePoints { get; set; }
        public decimal? GuestLegs { get; set; }
        public decimal? GuestPoints { get; set; }
        public bool IsOvertime { get; set; }
        public long? GuestTeamId { get; set; }
        public long? HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }
        public Team GuestTeam { get; set; }
        public long LeagueId { get; set; }
        public League League { get; set; }

        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;

        [NotMapped]
        public string GuestTeamName
        {
            get => GuestTeam?.Name ?? "Bye";
        }

        [NotMapped]
        public string HomeTeamName
        {
            get => HomeTeam?.Name ?? "Bye";
        }

        [NotMapped]
        public string Formatted => $"{HomeTeamName} : {GuestTeamName}";
        [NotMapped]
        public string Result => $"{(int?)HomePoints ?? 0} : {(int?)GuestPoints ?? 0}";
        [NotMapped]
        public string Legs => $"{HomeLegs ?? 0} : {GuestLegs ?? 0}";

        [NotMapped]
        public bool IsPlayed => HomePoints.HasValue || GuestPoints.HasValue;

        [NotMapped]
        public bool IsComplete => HomeTeamId.HasValue && GuestTeamId.HasValue;

        [NotMapped]
        public bool IsEmpty => !HomeTeamId.HasValue && !GuestTeamId.HasValue;

        public ICollection<Stats> Stats { get; set; } = new List<Stats>();

        public override bool Equals(object obj)
        {
            return obj is Match match &&
                   Id == match.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public bool Win(long? teamId)
        {
            bool result = false;
            if (teamId.HasValue)
            {
                result = ResolveMatchResult(teamId.Value) == MatchResult.Win;
            }
            return result;
        }

        public MatchResult ResolveMatchResult(long teamId)
        {
            var result = HomeTeamId == teamId
               ? ResolveInternal(HomePoints, GuestPoints)
               : ResolveInternal(GuestPoints, HomePoints);

            return result;
        }

        private MatchResult ResolveInternal(decimal? homePoints, decimal? guestPoints)
        {
            MatchResult result = MatchResult.Empty;

            if (homePoints.HasValue && guestPoints.HasValue)
            {
                if (homePoints > guestPoints)
                {
                    result = IsOvertime ? MatchResult.OvertimeWin : MatchResult.Win;
                }
                else if (homePoints < guestPoints)
                {
                    result = IsOvertime ? MatchResult.OverimeLose : MatchResult.Loss;
                }
                else
                {
                    if (homePoints > 0 || guestPoints > 0)
                    {
                        result = MatchResult.Draw;
                    }
                }
            }
            else
            {
                if (homePoints.HasValue)
                {
                    result = MatchResult.Win;
                }
                else if (guestPoints.HasValue)
                {
                    result = MatchResult.Loss;
                }
            }

            return result;
        }
    }
}
