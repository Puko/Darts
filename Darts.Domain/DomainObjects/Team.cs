using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Darts.Domain.DomainObjects
{
    public class Team
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        public DateTimeOffset Created { get; set; }

        [NotMapped]
        public int Wins { get; set; }
        [NotMapped]
        public int Looses { get; set; }
        [NotMapped]
        public int Draws { get; set; }
        [NotMapped]
        public int OverimeLosses { get; set; }
        [NotMapped]
        public int OverimeWins { get; set; }
        [NotMapped]
        public int Points { get; private set; }
        public int Played => Wins + Looses + Draws + OverimeLosses + OverimeWins;
        [NotMapped]
        public int HomeWins { get; set; }
        [NotMapped]
        public int HomeLooses { get; set; }
        [NotMapped]
        public int HomeDraws { get; set; }
        [NotMapped]
        public int HomeOvertimeLosses { get; set; }
        [NotMapped]
        public int HomeOvertimeWins { get; set; }
        [NotMapped]
        public int HomePoints { get; private set; }
        public int HomePlayed => HomeWins + HomeLooses + HomeDraws + HomeOvertimeLosses + HomeOvertimeWins;
        [NotMapped]
        public int AwayWins { get; set; }
        [NotMapped]
        public int AwayLooses { get; set; }
        [NotMapped]
        public int AwayDraws { get; set; }
        [NotMapped]
        public int AwayOvertimeLosses { get; set; }
        [NotMapped]
        public int AwayOvertimeWins { get; set; }

        [NotMapped]
        public int AwayPoints { get; private set; }
        [NotMapped]
        public int AwayPlayed => AwayWins + AwayLooses + AwayDraws + AwayOvertimeLosses + AwayOvertimeWins;

        public virtual ICollection<LeagueTeam> LeagueTeams { get; set; } = new List<LeagueTeam>();
        public ICollection<LeagueTeamPlayer> LeagueTeamPlayers { get; set; } = new List<LeagueTeamPlayer>();
        public IEnumerable<Player> GetPlayers(long leagueId)
        {
            List<Player> result = new List<Player>();
            if (LeagueTeamPlayers.Any())
            {
                result = LeagueTeamPlayers.Where(x => x.LeagueId == leagueId && x.TeamId == Id).Select(x => x.Player).ToList();
            }

            return result;
        }

        public decimal WinningPercentage
        {
            get
            {
                decimal result = 0;
                if (Played > 0)
                {
                    result = (2 * Wins + Draws) / (2 * Played) * 100;
                }
                return result;
            }
        }

        public void FillPoints(League league, List<Match> allMatches)
        {
            var awayMatches = allMatches.Where(x => x.GuestTeamId.HasValue && x.GuestTeamId == Id).ToList();
            var homeMatches = allMatches.Where(x => x.HomeTeamId.HasValue && x.HomeTeamId == Id).ToList();
            var matches = awayMatches.Concat(homeMatches);

            Wins = matches.Count(x => x.ResolveMatchResult(Id) == MatchResult.Win);
            Looses = matches.Count(x => x.ResolveMatchResult(Id) == MatchResult.Loss);
            Draws = matches.Count(x => x.ResolveMatchResult(Id) == MatchResult.Draw);
            OverimeLosses = matches.Count(x => x.ResolveMatchResult(Id) == MatchResult.OverimeLose);
            OverimeWins = matches.Count(x => x.ResolveMatchResult(Id) == MatchResult.OvertimeWin);

            AwayWins = awayMatches.Count(x => x.ResolveMatchResult(Id) == MatchResult.Win);
            AwayLooses = awayMatches.Count(x => x.ResolveMatchResult(Id) == MatchResult.Loss);
            AwayDraws = awayMatches.Count(x => x.ResolveMatchResult(Id) == MatchResult.Draw);
            AwayOvertimeLosses = awayMatches.Count(x => x.ResolveMatchResult(Id) == MatchResult.OverimeLose);
            AwayOvertimeWins = awayMatches.Count(x => x.ResolveMatchResult(Id) == MatchResult.OvertimeWin);

            HomeWins = homeMatches.Count(x => x.ResolveMatchResult(Id) == MatchResult.Win);
            HomeLooses = homeMatches.Count(x => x.ResolveMatchResult(Id) == MatchResult.Loss);
            HomeDraws = homeMatches.Count(x => x.ResolveMatchResult(Id) == MatchResult.Draw);
            HomeOvertimeLosses = homeMatches.Count(x => x.ResolveMatchResult(Id) == MatchResult.OverimeLose);
            HomeOvertimeWins = homeMatches.Count(x => x.ResolveMatchResult(Id) == MatchResult.OvertimeWin);

            var pointsForWin = Wins * league.PointsPerWin;
            var pointsForLoose = Looses * league.PointsPerLoose;
            var pointsForDraw = Draws * league.PointsForDraw;
            var pointsForOvertimeLoose = OverimeLosses * league.PointsPerOvertimeLoose;
            var pointsForOvertimeWin = OverimeWins * league.PointsPerOvertimeWin;

            Points = pointsForWin + pointsForLoose + pointsForDraw + pointsForOvertimeWin + pointsForOvertimeLoose;
        }

        public override bool Equals(object obj)
        {
            return obj is Team team &&
                   Id == team.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
