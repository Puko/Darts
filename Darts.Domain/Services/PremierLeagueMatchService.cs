using Darts.Contract;
using Darts.Domain.Abstracts.Factories;
using Darts.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Match = Darts.Domain.DomainObjects.Match;
using PremierLeagueMatch = Darts.Domain.DomainObjects.PremierLeagueMatch;

namespace Darts.Domain.Services
{
    public class PremierLeagueMatchService : BaseService<PremierLeagueMatch>
   {
      public PremierLeagueMatchService(IGenericContextFactory dbContextFactory) : base(dbContextFactory)
      {
      }

      public override PremierLeagueMatch Get(long id)
      {
         return Repository.GetSingle<PremierLeagueMatch>(x => x.Id == id);
      }

      public PremierLeagueMatch GetMatch(long premierLeagueId, long matchId)
      {
         return GetMatchesInternal(premierLeagueId, null, x => x.PremierLeagueId == premierLeagueId && x.Id == matchId).FirstOrDefault();
      }

      public byte[] GetEnrollment(long matchId) 
      {
         return Repository.GetAll<Enrollment>().Where(x => x.PremierLeagueMatchId == matchId).Select(x => x.Data).FirstOrDefault();
      }

      public ValidationResult<PremierLeagueMatch, PremierLeagueMatchValidationResult> Add(long userId, PremierLeagueMatch match)
      {
         EnsureLeagueOwner(userId, match.PremierLeagueId);

         ValidationResult<PremierLeagueMatch, PremierLeagueMatchValidationResult> result = new ValidationResult<PremierLeagueMatch, PremierLeagueMatchValidationResult>(match, PremierLeagueMatchValidationResult.InvalidDate);
         if (match.Date != DateTimeOffset.MinValue && match.Date != DateTimeOffset.MaxValue)
         {
            var existingMatch = Repository.GetSingle<PremierLeagueMatch>(x => x.Date == match.Date);
            if(existingMatch == null)
            {
               if(match.IsEmpty)
               {
                  result = new ValidationResult<PremierLeagueMatch, PremierLeagueMatchValidationResult>(match, PremierLeagueMatchValidationResult.EmptyMatch);
               }
               else
               {
                  if(match.HomePlayerId == match.GuestPlayerId)
                  {
                     result = new ValidationResult<PremierLeagueMatch, PremierLeagueMatchValidationResult>(match, PremierLeagueMatchValidationResult.BothPlayersAreTheSame);
                  }
                  else
                  {
                     Transaction(r =>
                     {
                        match.Date = match.Date.Date;
                        r.Add(match);
                     });
                     result = new ValidationResult<PremierLeagueMatch, PremierLeagueMatchValidationResult>(match, PremierLeagueMatchValidationResult.Success);
                  }
               }
            }
            else
            {
               result = new ValidationResult<PremierLeagueMatch, PremierLeagueMatchValidationResult>(match, PremierLeagueMatchValidationResult.MatchIsAlreadyScheduledForSelectedDate);
            }
         }

         return result;
      }

      public ValidationResult<PremierLeagueMatch, PremierLeagueMatchValidationResult> Edit(long userId, PremierLeagueMatch match)
      {
         EnsureLeagueOwner(userId, match.PremierLeagueId);

         ValidationResult<PremierLeagueMatch, PremierLeagueMatchValidationResult> result = new ValidationResult<PremierLeagueMatch, PremierLeagueMatchValidationResult>(match, PremierLeagueMatchValidationResult.EmptyMatch);
         var existingMatch = Get(match.Id);
         if (existingMatch != null)
         {
            if (match.IsEmpty)
            {
               result = new ValidationResult<PremierLeagueMatch, PremierLeagueMatchValidationResult>(match, PremierLeagueMatchValidationResult.MatchIsAlreadyScheduledForSelectedDate);
            }
            else if (match.HomePlayerId == match.GuestPlayerId)
            {
               result = new ValidationResult<PremierLeagueMatch, PremierLeagueMatchValidationResult>(match, PremierLeagueMatchValidationResult.BothPlayersAreTheSame);
            }
            else
            {
               Transaction(r =>
               {
                  r.Update(match);
               });
               result = new ValidationResult<PremierLeagueMatch, PremierLeagueMatchValidationResult>(match, PremierLeagueMatchValidationResult.Success);
            }
         }

         return result;
      }

      public void UpdateEnrollment(byte[] data, long matchId)
      {
         Transaction(r =>
         {
            var enrollment = r.GetSingle<Enrollment>(x => x.PremierLeagueMatchId == matchId);
            if(enrollment != null)
            {
               enrollment.Data = data;
               r.Update(enrollment);
            }
            else 
            {
               r.Add(new Enrollment
               {
                  Data = data,
                  PremierLeagueMatchId = matchId
               });
            }
         });
      }

      public IEnumerable<PremierLeagueMatch> GetPlayerMatches(long premierLeagueId, long playerId)
      {
         return GetMatchesInternal(premierLeagueId, null, x => x.PremierLeagueId == premierLeagueId && (x.HomePlayerId == playerId || x.GuestPlayerId == playerId));
      }

      public IEnumerable<PremierLeagueMatch> GetMatches(long premierLeagueId, DateTimeOffset date)
      {
         var dateToSelect = date.Date;
         return GetMatchesInternal(premierLeagueId, null, x => dateToSelect == EF.Property<DateTimeOffset>(x, nameof(Match.Date)).Date);
      }

      public IEnumerable<PremierLeagueMatch> GetMatches(long premierLeagueId)
      {
         return GetMatchesInternal(premierLeagueId, null, x => x.PremierLeagueId == premierLeagueId).OrderBy(x => x.Date);
      }

      public RoundValidationResult CheckRound(IEnumerable<PremierLeagueMatch> matches)
      {
         RoundValidationResult result = RoundValidationResult.EmptyRound;

         if (matches.Any())
         {
            result = RoundValidationResult.EmptyMatch;

            if (!matches.Any(x => !x.HomePlayerId.HasValue && !x.GuestPlayerId.HasValue))
            {
               result = RoundValidationResult.DuplicateMatch;

               var duplicates = matches.GroupBy(x => new { x.HomePlayerId, x.GuestPlayerId }).Any(x => x.Count() > 1);

               if (!duplicates)
               {
                  result = RoundValidationResult.Invalid;
                  var againstSelf = matches.GroupBy(x => new { x.HomePlayerId, x.GuestPlayerId }).Any(x => x.Key.HomePlayerId == x.Key.GuestPlayerId);

                  if (!againstSelf)
                  {
                     result = RoundValidationResult.Success;
                  }
               }
            }
         }

         return result;
      }

      private IEnumerable<PremierLeagueMatch> GetMatchesInternal(long leagueId, Filter pageInfo, Expression<Func<PremierLeagueMatch, bool>> where)
      {
         return GetAll(where, x => x.Include(x => x.HomePlayer).Include(x => x.GuestPlayer), x => x.OrderBy(x => x.Date), pageInfo).Where(x => x.PremierLeagueId == leagueId);
      }
   }
}