using System.Collections.Generic;
using System.Linq;
using Darts.Domain.Abstracts.Factories;
using Darts.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System;
using Newtonsoft.Json;
using Darts.Domain.Abstracts;
using Darts.Contract;
using LeagueTeamPlayer = Darts.Domain.DomainObjects.LeagueTeamPlayer;
using Player = Darts.Domain.DomainObjects.Player;

namespace Darts.Domain
{
    public class PlayerService : BaseService<Player>
    {
        private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;

        public PlayerService(IGenericContextFactory factory, ILeagueTeamPlayerRepository leagueTeamPlayerRepository)
            : base(factory)
        {
            _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
        }

        public override Player Get(long id)
        {
            return Get(x => x.Id == id);
        }

        public Player Get(string firstName, string lastName)
        {
            return Get(x => x.FirstName == firstName && x.LastName == lastName);
        }

        public Player Get(string identifier)
        {
            return Get(x => x.Identifier == identifier);
        }

        public Player GetPlayer(long playerId, PlayerInclusionInfo playerInclusionInfo)
        {
            return Get(x => x.Id == playerId, include: x => Include(null, x, playerInclusionInfo));
        }

        public Player GetPlayer(long leagueId, long playerId, PlayerInclusionInfo playerInclusionInfo)
        {
            return Get(x => x.Id == playerId, include: x => Include(leagueId, x, playerInclusionInfo));
        }

        public PageResult<Player> GetAllPlayers(Filter filter, PlayerInclusionInfo playerInclusionInfo)
        {
            return new PageResult<Player>
            {
                Items = GetAll(GetSearchExpression(filter.SearchText), x => Include(null, x, playerInclusionInfo), x => x.OrderBy(filter, x => x.Id), filter),
                Count = Count()
            };
        }

        public PageResult<Player> GetPlayersForLeague(long leagueId, Filter filter, PlayerInclusionInfo playerInclusionInfo)
        {
            IEnumerable<Player> players = GetAll(GetSearchExpression(filter.SearchText), x => Include(leagueId, x, playerInclusionInfo), x => x.OrderBy(x => x.Id), filter)
                                             .Where(x => x.LeagueTeamPlayers.Any(x => x.LeagueId == leagueId))
                                             .ToList().OrderByDescending(x => x.Wins).ThenByDescending(p => p.Percentage).ThenByDescending(x => x.Games);

            return new PageResult<Player>
            {
                Items = players,
                Count = Count()
            };
        }

        public ValidationResult<Player, PlayerValidationResult> Add(long userId, Player player)
        {
            ValidationResult<Player, PlayerValidationResult> result = new ValidationResult<Player, PlayerValidationResult>(player, PlayerValidationResult.PlayerAlreadyExist);
            var existingPlayer = Get(player.Identifier);
            if (existingPlayer == null)
            {
                Transaction(r =>
                {
                    player.Created = DateTimeOffset.UtcNow;
                    player.UserId = userId;
                    r.Add(player);
                });

                result = new ValidationResult<Player, PlayerValidationResult>(player, PlayerValidationResult.Success);
            }

            return result;
        }

        public ValidationResult<Player, PlayerValidationResult> Edit(long userId, Player player)
        {
            ValidationResult<Player, PlayerValidationResult> result = new ValidationResult<Player, PlayerValidationResult>(player, PlayerValidationResult.PlayerAlreadyExist);
            var existingPlayer = Get(player.Identifier);

            if (existingPlayer == null || existingPlayer.Id == player.Id)
            {
                if (existingPlayer == null)
                {
                    existingPlayer = Get(player.Id);
                }

                var diffObj = new JsonDiffPatch.JsonDiffer();
                var patchDocument = diffObj.Diff(JsonConvert.SerializeObject(new { existingPlayer.FirstName, existingPlayer.LastName, existingPlayer.Identifier }), JsonConvert.SerializeObject(new { player.FirstName, player.LastName, player.Identifier }), false);

                EditPlayerRecord editPlayerRecord = new EditPlayerRecord
                {
                    UserId = userId,
                    PlayerId = player.Id,
                    Diff = patchDocument.ToString()
                };

                Transaction(r =>
                {
                    player.UserId = userId;
                    r.Update(player);
                    r.Add(editPlayerRecord);
                });

                result = new ValidationResult<Player, PlayerValidationResult>(player, PlayerValidationResult.Success);
            }

            return result;
        }

        public ValidationResult<LeagueTeamPlayer, PlayerValidationResult> AssignPlayer(long playerId, long teamId, long leagueId)
        {
            ValidationResult<LeagueTeamPlayer, PlayerValidationResult> result;
            var leagueTeamPlayer = GetLeagueTeamPlayer(playerId, teamId, leagueId);
            if (leagueTeamPlayer == null)
            {
                _leagueTeamPlayerRepository.Add(new LeagueTeamPlayer { LeagueId = leagueId, TeamId = teamId, PlayerId = playerId });
                _leagueTeamPlayerRepository.Save();

                leagueTeamPlayer = GetLeagueTeamPlayer(playerId, teamId, leagueId);
                result = new ValidationResult<LeagueTeamPlayer, PlayerValidationResult>(leagueTeamPlayer, PlayerValidationResult.Success);
            }
            else
            {
                result = new ValidationResult<LeagueTeamPlayer, PlayerValidationResult>(leagueTeamPlayer, PlayerValidationResult.PlayerAlreadyAssignedInThisLeague);
            }

            return result;
        }

        public void DeletePlayerFromLeagueAndTeam(long userId, long leagueId, long teamId, long playerId)
        {
            EnsureLeagueOwner(userId, leagueId);

            var leagueTeamPlayer = GetLeagueTeamPlayer(playerId, teamId, leagueId);
            if (leagueTeamPlayer != null)
            {
                EditPlayerRecord editPlayerRecord = new EditPlayerRecord
                {
                    UserId = userId,
                    PlayerId = playerId,
                    Diff = $"Removed from league {leagueId}, team {teamId}"
                };

                Transaction(r =>
                {
                    r.Delete(leagueTeamPlayer);
                    r.Add(editPlayerRecord);
                });
            }
        }

        public IEnumerable<Player> GetPlayers(long teamId, long leagueId, Filter pageInfo, PlayerInclusionInfo playerInclusionInfo)
        {
            IEnumerable<Player> players = GetAll(x => x.LeagueTeamPlayers.Any(x => x.TeamId == teamId && x.LeagueId == leagueId),
                                x => Include(leagueId, x, playerInclusionInfo), x => x.OrderBy(p => p.LastName), pageInfo);

            return players.ToList().OrderByDescending(x => x.Wins).ThenByDescending(p => p.Percentage).ThenByDescending(x => x.Games);
        }

        public LeagueTeamPlayer GetLeagueTeamPlayer(long playerId, long teamId, long leagueId)
        {
            var ltp = Repository.GetSingle<LeagueTeamPlayer>(x => x.PlayerId == playerId && x.LeagueId == leagueId && x.TeamId == teamId, x => x.Include(y => y.Team).Include(x => x.League).Include(x => x.Player));
            return ltp;
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

        private IIncludableQueryable<Player, object> Include(long? leagueId, IQueryable<Player> x, PlayerInclusionInfo inclusionInfo)
        {
            IIncludableQueryable<Player, object> result = x.Include(x => x.LeagueTeamPlayers);

            if (leagueId.HasValue)
            {
                result = x.Include(x => x.LeagueTeamPlayers.Where(x => x.LeagueId == leagueId)).ThenInclude(x => x.Team);
            }

            if (inclusionInfo.User)
            {
                result = result.Include(x => x.User);
            }

            if (inclusionInfo.Stats)
            {
                result = result.Include(x => x.Stats);
            }

            return result;
        }
    }
}
