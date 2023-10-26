using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Darts.Domain.DomainObjects
{
    public class Player
    {
        public long Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Identifier { get; set; }
        public DateTimeOffset Created { get; set; }
        [NotMapped]
        public string FullName => FirstName != null && LastName != null ? $"{FirstName} {LastName}" : null;
        public decimal Wins => Stats.Sum(x => x.Points);
        public decimal Games => Stats.Sum(x => x.Games);

        public long UserId { get; set; }
        public User User { get; set; }

        public decimal Looses => Games - Wins;

        public decimal Percentage
        {
            get
            {
                decimal result = 0;

                var games = Games;
                if (games > 0)
                {
                    result = Wins / games * 100;
                }

                return result;
            }
        }

        [NotMapped]
        public int PremierLeagueWins { get; set; }
        [NotMapped]
        public int PremierLeagueLooses { get; set; }
        [NotMapped]
        public int PremierLeagueDraws { get; set; }
        [NotMapped]
        public int PremierLeaguePoints { get; private set; }
        [NotMapped]
        public List<PremierLeagueMatch> PremierLeagueMatches { get; set; }

        [NotMapped]
        public string PremierLeagueScore { get; set; }

        [NotMapped]
        public decimal PremierLeagueAverage
        {
            get
            {
                decimal result = 0;
                if (PremierLeagueMatches != null)
                {
                    var homeAverage = PremierLeagueMatches.Where(x => x.HomePlayerId == Id).Select(x => x.HomeAverage).ToList();
                    var guestAverage = PremierLeagueMatches.Where(x => x.GuestPlayerId == Id).Select(x => x.GuestAverage).ToList();
                    var allAverages = homeAverage.Concat(guestAverage);

                    if (allAverages.Any())
                    {
                        result = Math.Round(allAverages.Average(), 2);
                    }
                }

                return result;
            }
        }

        public int PremierLeaguesGames => PremierLeagueMatches?.Count ?? 0;


        public ICollection<LeagueTeamPlayer> LeagueTeamPlayers { get; set; } = new List<LeagueTeamPlayer>();
        public ICollection<Stats> Stats { get; set; } = new List<Stats>();
        public ICollection<PremierLeaguePlayer> PremierLeaguePlayers { get; set; } = new List<PremierLeaguePlayer>();

        public void FillPremierLeaguePoints(PremierLeague league, List<PremierLeagueMatch> matches)
        {
            var homeMatches = matches.Where(x => x.HomePlayerId.HasValue && x.HomePlayerId == Id).ToList();
            var awayMatches = matches.Where(x => x.GuestPlayerId.HasValue && x.GuestPlayerId == Id).ToList();

            PremierLeagueMatches = matches;
            PremierLeagueWins = PremierLeagueMatches.Count(x => x.ResolveMatchResult(Id) == MatchResult.Win);
            PremierLeagueLooses = PremierLeagueMatches.Count(x => x.ResolveMatchResult(Id) == MatchResult.Loss);
            PremierLeagueDraws = PremierLeagueMatches.Count(x => x.ResolveMatchResult(Id) == MatchResult.Draw);

            decimal myLegs = PremierLeagueMatches.Where(x => x.HomePlayerId == Id).Sum(x => x.HomeLegs) +
                        PremierLeagueMatches.Where(x => x.GuestPlayerId == Id).Sum(x => x.GuestLegs);

            decimal guestLegs = PremierLeagueMatches.Where(x => x.HomePlayerId != Id).Sum(x => x.HomeLegs) +
                    PremierLeagueMatches.Where(x => x.GuestPlayerId != Id).Sum(x => x.GuestLegs); ;

            PremierLeagueScore = $"{(int)myLegs} : {(int)guestLegs}";

            var pointsForWin = PremierLeagueWins * league.PointsPerWin;
            var pointsForLoose = PremierLeagueLooses * league.PointsPerLoose;
            var pointsForDraw = PremierLeagueDraws * league.PointsForDraw;

            PremierLeaguePoints = pointsForWin + pointsForLoose + pointsForDraw;
        }

        public override bool Equals(object obj)
        {
            return obj is Player player &&
                   Id == player.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
