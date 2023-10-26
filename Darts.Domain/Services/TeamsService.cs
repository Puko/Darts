using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Darts.Contract;
using Darts.Domain.Abstracts;
using Darts.Domain.Abstracts.Factories;
using Darts.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using League = Darts.Domain.DomainObjects.League;
using LeagueTeam = Darts.Domain.DomainObjects.LeagueTeam;
using Match = Darts.Domain.DomainObjects.Match;
using Team = Darts.Domain.DomainObjects.Team;

namespace Darts.Domain
{
    public class TeamsService : BaseService<Team>
   {
      private readonly ILeagueTeamRepository _leagueTeamRepository;
      private readonly IMatchRepository _matchRepository;

      public TeamsService(IGenericContextFactory factory, ILeagueTeamRepository leagueTeamRepository, IMatchRepository matchRepository)
          : base(factory)
      {
         _leagueTeamRepository = leagueTeamRepository;
         _matchRepository = matchRepository;
      }

      public Team GetTeam(long teamId, TeamIncusionInfo teamIncusionInfo)
      {
         return Get(x => x.Id == teamId, include: x => Include(null, x, teamIncusionInfo));
      }

      public Team GetTeam(long leagueId, long teamId, TeamIncusionInfo teamIncusionInfo)
      {
         var team = Get(x => x.Id == teamId, include: x => Include(leagueId, x, teamIncusionInfo));
         return team;
      }

      public override Team Get(long id)
      {
         return Get(x => x.Id == id);
      }

      public Team Get(string name)
      {
         return Get(x => x.Name == name);
      }

      public IEnumerable<Team> GetTeams(long leagueId, Filter pageInfo, TeamIncusionInfo teamIncusionInfo, string search = null)
      {
         IEnumerable<Team> teams = GetTeamsInternal(leagueId, pageInfo, teamIncusionInfo, search);

         if (teamIncusionInfo?.Matches == true)
         {
            var leagueMatches = _matchRepository.GetAll(x => x.LeagueId == leagueId).ToList();

            var league = Repository.GetSingle<League>(x => x.Id == leagueId);
            foreach (Team team in teams)
            {
               team.FillPoints(league, leagueMatches);
            }
         }

         return teams;
      }

      public PageResult<Team> GetAllTeams(Filter filter, TeamIncusionInfo teamIncusionInfo)
      {
         return new PageResult<Team>
         {
            Items = GetAll(GetSearchExpression(filter.SearchText), x => Include(null, x, teamIncusionInfo), x => x.OrderBy(filter, x => x.Id), filter),
            Count = Count()
         };
      }

      public ValidationResult<Team, TeamValidationResult> Add(Team team)
      {
         ValidationResult<Team, TeamValidationResult> result;
         var existingPlayer = Get(team.Name);

         if (existingPlayer == null)
         {
            Transaction(r =>
            {
               team.Created = DateTimeOffset.UtcNow;
               r.Add(team);
            });

            result = new ValidationResult<Team, TeamValidationResult>(team, TeamValidationResult.Success);
         }
         else
         {
            result = new ValidationResult<Team, TeamValidationResult>(TeamValidationResult.TeamAlreadyExist);
         }

         return result;
      }

      public ValidationResult<Team, TeamValidationResult> Edit(long userId, Team team)
      {
         ValidationResult<Team, TeamValidationResult> result;
         var existingTeam = Get(team.Name);

         if (existingTeam == null || existingTeam.Id == team.Id)
         {
            if (existingTeam == null)
            {
               existingTeam = Get(team.Id);
            }

            var diffObj = new JsonDiffPatch.JsonDiffer();
            var patchDocument = diffObj.Diff(JsonConvert.SerializeObject(new { existingTeam.Name, existingTeam.Address, existingTeam.City }), JsonConvert.SerializeObject(new { team.Name, team.Address, team.City }), false);

            EditTeamRecord editPlayerRecord = new EditTeamRecord
            {
               UserId = userId,
               TeamId = team.Id,
               Diff = patchDocument.ToString()
            };

            Transaction(r =>
            {
               r.Update(team);
               r.Add(editPlayerRecord);
            });

            result = new ValidationResult<Team, TeamValidationResult>(team, TeamValidationResult.Success);
         }
         else
         {
            result = new ValidationResult<Team, TeamValidationResult>(TeamValidationResult.TeamAlreadyExist);
         }

         return result;
      }

      public ValidationResult<LeagueTeam, TeamValidationResult> AssignTeam(long teamId, long leagueId)
      {
         ValidationResult<LeagueTeam, TeamValidationResult> result;
         var leagueTeamPlayer = GetLeagueTeam(teamId, leagueId);
         if (leagueTeamPlayer == null)
         {
            _leagueTeamRepository.Add(new LeagueTeam { LeagueId = leagueId, TeamId = teamId });
            _leagueTeamRepository.Save();

            leagueTeamPlayer = GetLeagueTeam(teamId, leagueId);
            result = new ValidationResult<LeagueTeam, TeamValidationResult>(leagueTeamPlayer, TeamValidationResult.Success);
         }
         else
         {
            result = new ValidationResult<LeagueTeam, TeamValidationResult>(leagueTeamPlayer, TeamValidationResult.TeamIsAlreadyInThisLeague);
         }

         return result;
      }

      public void DeleteTeamFromLeague(long userId, long leagueId, long teamId)
      {
         EnsureLeagueOwner(userId, leagueId);

         var leagueTeamPlayer = GetLeagueTeam(teamId, leagueId);
         if (leagueTeamPlayer != null)
         {
            EditTeamRecord editPlayerRecord = new EditTeamRecord
            {
               UserId = userId,
               TeamId = teamId,
               Diff = $"Removed from league {leagueId}"
            };

            Transaction(r =>
            {
               r.Delete(leagueTeamPlayer);
               r.Add(editPlayerRecord);
            });
         }
      }

      public void DeleteTeam(long userId, long leagueId, long teamId)
      {
         EnsureLeagueOwner(userId, leagueId);

         var team = Get(x => x.Id == teamId, x => Include(leagueId, x, TeamIncusionInfo.Empty));
         var leagueTeam = team.LeagueTeams.FirstOrDefault(x => x.LeagueId == leagueId && x.TeamId == teamId);
         var leagueTeamPlayer = team.LeagueTeamPlayers.FirstOrDefault(x => x.LeagueId == leagueId && x.TeamId == teamId);
         var matches = Repository.GetAll<Match>(x => (x.GuestTeamId == teamId || x.HomeTeamId == teamId) && x.LeagueId == leagueId);

         Transaction(r =>
         {
            foreach (var match in matches)
            {
               r.Delete(match);
            }

            if (leagueTeam != null)
            {
               r.Delete(leagueTeam);
            }

            if (leagueTeamPlayer != null)
            {
               r.Delete(leagueTeamPlayer);
            }
         });
      }

      public LeagueTeam GetLeagueTeam(long teamId, long leagueId)
      {
         var lt = Repository.GetSingle<LeagueTeam>(x => x.TeamId == teamId && x.LeagueId == leagueId, x => x.Include(y => y.Team)
         .Include(x => x.League));
         return lt;
      }


      private IEnumerable<Team> GetTeamsInternal(long leagueId, Filter pageInfo, TeamIncusionInfo teamIncusionInfo, string search = null)
      {
         return Execute(r =>
         {
            Expression<Func<Team, bool>> searchExpression = GetSearchExpression(search);

            return GetAll(searchExpression, include: x => Include(leagueId, x, teamIncusionInfo), x => x.OrderBy(t => t.Name), pageInfo)
                    .Where(x => x.LeagueTeams.Any(lt => lt.LeagueId == leagueId))
                    .ToList()
                    .OrderByDescending(x => x.Points).ThenByDescending(x => x.Wins).ThenBy(x => x.Id);

         });
      }

      private static Expression<Func<Team, bool>> GetSearchExpression(string search)
      {
         if (search == null)
         {
            return null;
         }

         search = search.ToLowerInvariant();
         return x => x.Name.ToLower().StartsWith(search) ||
                  x.City.ToLower().StartsWith(search) || x.Address.ToLower().StartsWith(search);

      }

      private IIncludableQueryable<Team, object> Include(long? leagueId, IQueryable<Team> x, TeamIncusionInfo incusionInfo)
      {
         IIncludableQueryable<Team, object> result = x.Include(x => x.LeagueTeamPlayers).ThenInclude(x => x.Player).Include(x => x.LeagueTeams);

         if (leagueId.HasValue)
         {
            result = x.Include(x => x.LeagueTeamPlayers.Where(x => x.LeagueId == leagueId))
                                                          .ThenInclude(x => x.Player)
                                                          .Include(x => x.LeagueTeams.Where(l => l.LeagueId == leagueId));
         }

         return result;
      }
   }
}
