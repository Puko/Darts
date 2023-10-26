using Darts.Contract;
using Darts.Domain.Abstracts;
using Darts.Domain.Abstracts.Factories;
using Darts.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Player = Darts.Domain.DomainObjects.Player;
using PremierLeague = Darts.Domain.DomainObjects.PremierLeague;
using PremierLeaguePlayer = Darts.Domain.DomainObjects.PremierLeaguePlayer;

namespace Darts.Domain.Services
{
    public class PremierLeaguePlayerService : BaseService<Player>
   {
      private readonly IPremierLeaguePlayerRepository _premierLeaguePlayerRepository;
      private readonly IPremierLeagueMatchRepository _premierLeagueMatchRepository;

      public PremierLeaguePlayerService(IGenericContextFactory dbContextFactory, IPremierLeaguePlayerRepository premierLeaguePlayerRepository,
          IPremierLeagueMatchRepository premierLeagueMatchRepository) : base(dbContextFactory)
      {
         _premierLeaguePlayerRepository = premierLeaguePlayerRepository;
         _premierLeagueMatchRepository = premierLeagueMatchRepository;
      }

      public IEnumerable<Player> GetPlayersForPremierLeague(long premierLeagueId, Filter pageInfo, PlayerInclusionInfo inclusionInfo, string search = null)
      {
         IEnumerable<Player> players = GetAll(GetSearchExpression(search),
                             x => IncludePremierLeague(premierLeagueId, x, inclusionInfo), x => x.OrderBy(x => x.LastName), pageInfo)
                             .Where(x => x.PremierLeaguePlayers.Any(x => x.PremierLeagueId == premierLeagueId));

         var result = players.ToList();
         if (inclusionInfo.PremierLeagueMatches)
         {
            var allMatches = _premierLeagueMatchRepository.GetAll(x => x.PremierLeagueId == premierLeagueId).ToList();

            var league = Repository.GetSingle<PremierLeague>(x => x.Id == premierLeagueId);
            result.ForEach(x => x.FillPremierLeaguePoints(league, allMatches));
         }

         return result.OrderByDescending(x => x.PremierLeaguePoints)
            .ThenByDescending(x => x.PremierLeagueWins)
            .ThenByDescending(p => p.PremierLeagueDraws)
            .ThenBy(x => x.Looses)
            .ThenByDescending(x => x.Games)
            .ThenByDescending(x => x.PremierLeagueAverage);
      }

      public void DeletePremierPlayer(long userId, long premierLeague, long playerId)
      {
         EnsureLeagueOwner(userId, premierLeague);

         var player = Get(x => x.Id == playerId, x => x.Include(t => t.PremierLeaguePlayers));
         var leaguePlayer = player.PremierLeaguePlayers.FirstOrDefault(x => x.PremierLeagueId == premierLeague && x.PlayerId == playerId);

         Transaction(r =>
         {
            if (leaguePlayer != null)
            {
               r.Delete(leaguePlayer);
            }
         });
      }

      public ValidationResult<PremierLeaguePlayer, PlayerValidationResult> AssignPlayer(long playerId, long leagueId)
      {
         ValidationResult<PremierLeaguePlayer, PlayerValidationResult> result;
         var leagueTeamPlayer = GetPremierLeaguePlayer(playerId, leagueId);
         if (leagueTeamPlayer == null)
         {
            _premierLeaguePlayerRepository.Add(new PremierLeaguePlayer { PremierLeagueId = leagueId, PlayerId = playerId });
            _premierLeaguePlayerRepository.Save();

            leagueTeamPlayer = GetPremierLeaguePlayer(playerId, leagueId);
            result = new ValidationResult<PremierLeaguePlayer, PlayerValidationResult>(leagueTeamPlayer, PlayerValidationResult.Success);
         }
         else
         {
            result = new ValidationResult<PremierLeaguePlayer, PlayerValidationResult>(leagueTeamPlayer, PlayerValidationResult.PlayerAlreadyAssignedInThisLeague);
         }

         return result;
      }

      public PremierLeaguePlayer GetPremierLeaguePlayer(long playerId, long leagueId)
      {
         var ltp = Repository.GetSingle<PremierLeaguePlayer>(x => x.PlayerId == playerId && x.PremierLeagueId == leagueId, x => x.Include(x => x.PremierLeague).Include(x => x.Player));
         return ltp;
      }

      public override Player Get(long id)
      {
         return Get(x => x.Id == id);
      }

      private static Expression<Func<Player, bool>> GetSearchExpression(string search)
      {
         if (search == null)
         {
            return null;
         }

         search = search.ToLowerInvariant();
         return x => x.FirstName.ToLower().StartsWith(search) ||
               x.LastName.ToLower().StartsWith(search) || x.Identifier.ToLower().StartsWith(search);

      }

      private IIncludableQueryable<Player, object> IncludePremierLeague(long premierLeagueId, IQueryable<Player> x, PlayerInclusionInfo playerInclusionInfo)
      {
         IIncludableQueryable<Player, object> result = x.Include(x => x.PremierLeaguePlayers.Where(x => x.PremierLeagueId == premierLeagueId));

         if (playerInclusionInfo.User)
         {
            result = result.Include(x => x.User);
         }

         return result;
      }
   }
}
