using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Darts.Contract;
using Darts.Domain.Abstracts.Factories;
using Darts.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OfferingSolutions.GenericEFCore.UnitOfWorkContext;
using Match = Darts.Domain.DomainObjects.Match;

namespace Darts.Domain
{
    public class MatchService : BaseService<Match>
    {
        public MatchService(IGenericContextFactory factory)
            : base(factory)
        {
        }

        public Match GetMatch(long leagueId, long matchId, MatchInclusionInfo inclusionInfo)
        {
            return GetMatchesInternal(leagueId, null, x => x.Id == matchId, inclusionInfo).FirstOrDefault();
        }

        public IEnumerable<Match> GetMatches(long leagueId, MatchInclusionInfo inclusionInfo)
        {
            return GetMatchesInternal(leagueId, new Filter { PageNumber = 1, PageSize = 1000 }, null, inclusionInfo);
        }

        public IEnumerable<Match> GetMatches(long leagueId, DateTimeOffset date, Filter pageInfo, MatchInclusionInfo inclusionInfo)
        {
            var dateToSelect = date.Date;
            return GetMatchesInternal(leagueId, pageInfo, x => dateToSelect == EF.Property<DateTimeOffset>(x, nameof(Match.Date)).Date, inclusionInfo);
        }

        public IEnumerable<Match> GetMatches(long leagueId, long teamId, Filter pageInfo, MatchInclusionInfo inclusionInfo)
        {
            return GetMatchesInternal(leagueId, pageInfo, x => x.HomeTeamId == teamId || x.GuestTeamId == teamId, inclusionInfo);
        }

        public override Match Get(long id)
        {
            return Get(x => x.Id == id);
        }

        public Match Get(long? homeTeamId, long? guestTeamId, DateTimeOffset date)
        {
            var dateToSelect = date.Date;
            return Get(x => x.HomeTeamId == homeTeamId && x.GuestTeamId == guestTeamId && dateToSelect == x.Date.Date);
        }
        public IEnumerable<ValidationResult<Match, MatchValidationResult>> AddMultiple(long userId, IEnumerable<Match> matches)
        {
            List<ValidationResult<Match, MatchValidationResult>> results = new List<ValidationResult<Match, MatchValidationResult>>();

            Transaction(r =>
            {
                foreach (var item in matches)
                {
                    results.Add(Add(userId, item, r));
                }

                if(results.All(x => x.Validation == MatchValidationResult.Success))
                {
                    r.Save();
                }
            });
            return results;
        }

        public ValidationResult<Match, MatchValidationResult> Add(long userId, Match match, IOsUnitOfWorkContext context = null)
        {
            EnsureLeagueOwner(userId, match.LeagueId);

            ValidationResult<Match, MatchValidationResult> result = new ValidationResult<Match, MatchValidationResult>(match, MatchValidationResult.InvalidDate);
            if (match.Date != DateTimeOffset.MinValue && match.Date != DateTimeOffset.MaxValue)
            {
                var existingMatch = Get(match.HomeTeamId, match.GuestTeamId, match.Date);
                if (existingMatch == null)
                {
                    if (match.IsEmpty)
                    {
                        result = new ValidationResult<Match, MatchValidationResult>(match, MatchValidationResult.EmptyMatch);
                    }
                    else if (match.HomeTeamId == match.GuestTeamId)
                    {
                        result = new ValidationResult<Match, MatchValidationResult>(match, MatchValidationResult.BothTeamsAreTheSame);
                    }
                    else
                    {
                        match.Date = match.Date.Date;

                        if (context == null)
                        {
                            Transaction(r =>
                            {
                                r.Add(match);
                            });
                        }
                        else
                        {
                            context.Add(match);
                        }

                        match = GetMatch(match.LeagueId, match.Id, new MatchInclusionInfo { Teams = true });
                        result = new ValidationResult<Match, MatchValidationResult>(match, MatchValidationResult.Success);
                    }
                }
                else
                {
                    result = new ValidationResult<Match, MatchValidationResult>(match, MatchValidationResult.MatchIsAlreadyScheduledForSelectedDate);
                }
            }

            return result;
        }

        public ValidationResult<Match, MatchValidationResult> Edit(long userId, Match match)
        {
            EnsureLeagueOwner(userId, match.LeagueId);

            ValidationResult<Match, MatchValidationResult> result = new ValidationResult<Match, MatchValidationResult>(match, MatchValidationResult.EmptyMatch);
            var existingMatch = Get(match.Id);
            if (existingMatch != null)
            {
                if (match.IsEmpty)
                {
                    result = new ValidationResult<Match, MatchValidationResult>(match, MatchValidationResult.MatchIsAlreadyScheduledForSelectedDate);
                }
                else if (match.HomeTeamId == match.GuestTeamId)
                {
                    result = new ValidationResult<Match, MatchValidationResult>(match, MatchValidationResult.BothTeamsAreTheSame);
                }
                else
                {
                    Transaction(r =>
                    {
                        r.Update(match);
                    });

                    match = GetMatch(match.LeagueId, match.Id, new MatchInclusionInfo { Teams = true });
                    result = new ValidationResult<Match, MatchValidationResult>(match, MatchValidationResult.Success);
                }
            }

            return result;
        }


        public void DeleteMatch(long userId, long leagueId, long matchId)
        {
            EnsureLeagueOwner(userId, leagueId);

            var match = Get(matchId);
            if(match != null)
            {
                Transaction(r =>
                {
                    r.Delete(match);
                });
            }
        }

        public byte[] GetEnrollment(long matchId)
        {
            return Repository.GetAll<Enrollment>().Where(x => x.MatchId == matchId).Select(x => x.Data).FirstOrDefault();
        }

        public void UpdateEnrollment(byte[] data, long matchId)
        {
            Transaction(r =>
            {
                var enrollment = r.GetSingle<Enrollment>(x => x.MatchId == matchId);
                if (enrollment != null)
                {
                    enrollment.Data = data;
                    r.Update(enrollment);
                }
                else
                {
                    r.Add(new Enrollment
                    {
                        Data = data,
                        MatchId = matchId
                    });
                }
            });
        }

        private IEnumerable<Match> GetMatchesInternal(long leagueId, Filter pageInfo, Expression<Func<Match, bool>> where, MatchInclusionInfo teamIncusionInfo)
        {
            return GetAll(where, include: x => Include(leagueId, x, teamIncusionInfo), x => x.OrderBy(x => x.Date), pageInfo).Where(x => x.LeagueId == leagueId).Select(x => new Match
            {
                Date = x.Date,
                GuestLegs = x.GuestLegs,
                GuestPoints = x.GuestPoints,
                GuestTeam = x.GuestTeam,
                GuestTeamId = x.GuestTeamId,
                HomeLegs = x.HomeLegs,
                HomePoints = x.HomePoints,
                HomeTeam = x.HomeTeam,
                HomeTeamId = x.HomeTeamId,
                Id = x.Id,
                IsOvertime = x.IsOvertime,
                LeagueId = x.LeagueId,
                Stats = x.Stats,
                League = x.League
            });
        }

        private IIncludableQueryable<Match, object> Include(long leagueId, IQueryable<Match> x, MatchInclusionInfo matchInclusionInfo)
        {
            IIncludableQueryable<Match, object> result = x.Include(x => x.Stats);

            if (matchInclusionInfo != null)
            {
                if (matchInclusionInfo.Teams)
                {
                    result = result.Include(x => x.HomeTeam).Include(x => x.GuestTeam);
                }

                if (matchInclusionInfo.Players)
                {
                    result = result.Include(x => x.HomeTeam)
                                   .ThenInclude(x => x.LeagueTeamPlayers.Where(x => x.LeagueId == leagueId))
                                   .ThenInclude(x => x.Player)
                                   .ThenInclude(x => x.Stats)
                                   .Include(x => x.GuestTeam)
                                   .ThenInclude(x => x.LeagueTeamPlayers.Where(x => x.LeagueId == leagueId))
                                   .ThenInclude(x => x.Player)
                                   .ThenInclude(x => x.Stats);
                }
            }

            return result;
        }
    }
}
