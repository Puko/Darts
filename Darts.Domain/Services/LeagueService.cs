using System;
using System.Collections.Generic;
using System.Linq;
using Darts.Contract;
using Darts.Domain.Abstracts.Factories;
using Darts.Domain.DomainObjects;
using Darts.Domain.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using League = Darts.Domain.DomainObjects.League;
using LeagueTeam = Darts.Domain.DomainObjects.LeagueTeam;
using LeagueTeamPlayer = Darts.Domain.DomainObjects.LeagueTeamPlayer;

namespace Darts.Domain
{
    public class LeagueService : BaseService<League>
    {
        public LeagueService(IGenericContextFactory factory)
            : base(factory, new LeagueValidator())
        {

        }

        public PageResult<League> GetLeagues(long userId, Filter pageInfo, LeagueIncusionInfo incusionInfo)
        {
            return new PageResult<League>
            {
                Items = GetAll(x => x.UserId == userId, include: x => Include(x, incusionInfo), x => x.OrderBy(pageInfo, x => x.Name), pageInfo),
                Count = Count(x => x.UserId == userId)
            };
        }

        public IEnumerable<League> GetLeagues(int year, LeagueIncusionInfo incusionInfo)
        {
            return GetAll(x => x.Created.Year == year, include: x => Include(x, incusionInfo));
        }

        public IEnumerable<League> GetLeagues(long teamId)
        {
            var repository = Repository;
            var leagues = repository.GetAll<LeagueTeam>(x => x.TeamId == teamId, include: x => x.Include(x => x.League).ThenInclude(x => x.User), null, null, null).Select(x => x.League);
            return leagues.ToList();
        }

        public PageResult<League> GetLeagues(LeagueIncusionInfo incusionInfo, Filter pageInfo)
        {
            return new PageResult<League>
            {
                Items = GetAll(null, include: x => Include(x, incusionInfo), x => x.OrderBy(pageInfo, x => x.Name), pageInfo),
                Count = Count()
            };
        }

        public League GetLeague(long leagueId, LeagueIncusionInfo incusionInfo)
        {
            var league = Get(x => x.Id == leagueId, include: x => Include(x, incusionInfo));
            return league;
        }

        public override League Get(long id)
        {
            return Get(x => x.Id == id);
        }

        public void DeleteLeague(long userId, long leagueId)
        {
            EnsureLeagueOwner(userId, leagueId);

            var league = Get(x => x.Id == leagueId, x => x.Include(t => t.LeagueTeams).Include(t => t.LeagueTeamPlayers));
            var leagueTeam = league.LeagueTeams.FirstOrDefault(x => x.LeagueId == leagueId);
            var leagueTeamPlayer = league.LeagueTeamPlayers.FirstOrDefault(x => x.LeagueId == leagueId);

            Transaction(r =>
            {
                if (leagueTeam != null)
                {
                    r.Delete(leagueTeam);
                }

                if (leagueTeamPlayer != null)
                {
                    r.Delete(leagueTeamPlayer);
                }

                r.Delete(league);
            });
        }

        public League CopyLeague(long userId, League league)
        {
            EnsureLeagueOwner(userId, league.Id);

            var newLeague = League.Copy(league);

            Transaction(r =>
            {
                var teams = r.GetAll<LeagueTeam>(x => x.LeagueId == league.Id);
                var assignments = r.GetAll<LeagueTeamPlayer>(x => x.LeagueId == league.Id);

                newLeague.LeagueTeams = teams.Select(x => new LeagueTeam
                {
                    TeamId = x.TeamId
                }).ToList();

                newLeague.LeagueTeamPlayers = assignments.Select(x => new LeagueTeamPlayer
                {
                    PlayerId = x.PlayerId,
                    TeamId = x.TeamId
                }).ToList();

                r.Add(newLeague);
            });

            return newLeague;
        }

        public ValidationResult<League, LeagueValidationResult> Add(long userId, League league)
        {
            var validationResult = Validate(league);
            ValidationResult<League, LeagueValidationResult> result = new ValidationResult<League, LeagueValidationResult>(league, LeagueValidationResult.MandatoryFieldsNotFilled);

            if (validationResult.IsValid)
            {
                var existingLeague = Get(x => x.Name == league.Name && x.UserId == userId);
                if (existingLeague == null)
                {
                    Transaction(r =>
                    {
                        league.Created = DateTimeOffset.UtcNow;
                        league.UserId = userId;

                        r.Add(league);
                    });

                    result = new ValidationResult<League, LeagueValidationResult>(league, LeagueValidationResult.Success);
                }
                else
                {
                    result = new ValidationResult<League, LeagueValidationResult>(league, LeagueValidationResult.LeagueWithNameAlreadyExist);
                }
            }
            else
            {
                result.ErrorMessages = validationResult.Errors.Select(x => x.ErrorMessage);
            }

            return result;
        }

        public ValidationResult<League, LeagueValidationResult> Edit(long userId, League league)
        {
            var validationResult = Validate(league);
            ValidationResult<League, LeagueValidationResult> result = new ValidationResult<League, LeagueValidationResult>(league, LeagueValidationResult.MandatoryFieldsNotFilled);

            if (validationResult.IsValid)
            {
                var existingLeague = Get(x => x.Name == league.Name && x.UserId == userId);
                if (existingLeague == null || existingLeague.Id == league.Id)
                {
                    Transaction(r =>
                    {
                        r.Update(league);
                    });

                    result = new ValidationResult<League, LeagueValidationResult>(league, LeagueValidationResult.Success);
                }
                else
                {
                    result = new ValidationResult<League, LeagueValidationResult>(league, LeagueValidationResult.LeagueWithNameAlreadyExist);
                }
            }
            else
            {
                result.ErrorMessages = validationResult.Errors.Select(x => x.ErrorMessage);
            }

            return result;
        }

        private IIncludableQueryable<League, object> Include(IQueryable<League> x, LeagueIncusionInfo incusionInfo)
        {
            IIncludableQueryable<League, object> result = x.Include(x => x.LeagueTeams).ThenInclude(x => x.Team);

            if (incusionInfo != null)
            {
                if (incusionInfo.User)
                {
                    result = result != null ? result.Include(x => x.User) : x.Include(x => x.User);
                }
            }

            return result;
        }
    }
}
