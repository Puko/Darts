using Darts.Contract;
using Darts.Domain.Abstracts.Factories;
using Darts.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Stats = Darts.Domain.DomainObjects.Stats;

namespace Darts.Domain
{
    public class StatsService : BaseService<Stats>
    {
        public StatsService(IGenericContextFactory factory)
            : base(factory)
        {

        }

        public override Stats Get(long id)
        {
            return Get(x => x.Id == id);
        }

        public IEnumerable<Stats> GetStats(long matchId)
        {
            return GetAll(x => x.MatchId == matchId, x => x.Include(s => s.Player));
        }

        public ValidationResult<IEnumerable<Stats>, StatsValidationResult> Edit(long userId, long leagueId, long matchId, IEnumerable<Stats> stats)
        {
            EnsureLeagueOwner(userId, leagueId);
            ValidationResult<IEnumerable<Stats>, StatsValidationResult> result = new ValidationResult<IEnumerable<Stats>, StatsValidationResult>(StatsValidationResult.Empty);

            if (stats.GroupBy(x => x.PlayerId).All(x => x.Count() == 1))
            {
                Transaction(r =>
                {
                    var dbStats = r.GetAll<Stats>(x => x.MatchId == matchId).AsNoTracking().ToList();
                    var statsToDelete = dbStats.Where(x => stats.All(y => y.Id != x.Id));

                    foreach (Stats stats in statsToDelete)
                    {
                        r.Delete<Stats>(x => x.Id == stats.Id);
                    }

                    foreach (Stats matchStat in stats)
                    {
                        if (matchStat.Id == 0)
                        {
                            if (!matchStat.IsEmpty)
                            {
                                r.Add(matchStat);
                            }
                        }
                        else
                        {   
                            if(matchStat.IsEmpty)
                            {
                                r.Delete(matchStat);
                            }
                            else 
                            {
                                r.Update(matchStat);
                            }
                        }
                    }
                });

                result = new ValidationResult<IEnumerable<Stats>, StatsValidationResult>(stats, StatsValidationResult.Success);
            }
            else
            {
                result = new ValidationResult<IEnumerable<Stats>, StatsValidationResult>(StatsValidationResult.OnePlayerHasMoreThanOneStatsInOneMatch);
            }

            return result;
        }

        public ValidationResult<Stats, StatsValidationResult> Edit(long userId, long leagueId, Stats stats)
        {
            EnsureLeagueOwner(userId, leagueId);

            ValidationResult<Stats, StatsValidationResult> result = new ValidationResult<Stats, StatsValidationResult>(StatsValidationResult.Empty);

            if (!stats.IsEmpty)
            {
                Transaction(r =>
                {
                    r.Update(stats);

                });

                result = new ValidationResult<Stats, StatsValidationResult>(stats, StatsValidationResult.Success);
            }

            return result;
        }

        public new ValidationResult<Stats, StatsValidationResult> Add(long userId, long leagueId, Stats stats)
        {
            EnsureLeagueOwner(userId, leagueId);

            ValidationResult<Stats, StatsValidationResult> result = new ValidationResult<Stats, StatsValidationResult>(StatsValidationResult.Empty);
            
            if (!stats.IsEmpty)
            {
                Transaction(r =>
                {
                    r.Add(stats);

                });

                result = new ValidationResult<Stats, StatsValidationResult>(stats, StatsValidationResult.Success);
            }

            return result;
        }

        public ValidationResult<IEnumerable<Stats>, StatsValidationResult> Add(long userId, long leagueId, IEnumerable<Stats> stats)
        {
            EnsureLeagueOwner(userId, leagueId);

            ValidationResult<IEnumerable<Stats>, StatsValidationResult> result = new ValidationResult<IEnumerable<Stats>, StatsValidationResult>(StatsValidationResult.Empty);
            if (stats.All(x => !x.IsEmpty))
            {
                if (stats.GroupBy(x => x.PlayerId).All(x => x.Count() == 1))
                {
                    Transaction(r =>
                    {
                        foreach (Stats s in stats.Where(x => !x.IsEmpty))
                        {
                            r.Add(s);
                        }
                    });

                    result = new ValidationResult<IEnumerable<Stats>, StatsValidationResult>(stats, StatsValidationResult.Success);
                }
                else
                {
                    result = new ValidationResult<IEnumerable<Stats>, StatsValidationResult>(StatsValidationResult.OnePlayerHasMoreThanOneStatsInOneMatch);
                }
            }

            return result;
        }

        public new void Delete(long userId, long leagueId, long matchId)
        {
            EnsureLeagueOwner(userId, leagueId);
            var stats = GetAll(x => x.MatchId == matchId);

            Transaction(r =>
            {
                foreach (var item in stats)
                {
                    r.Delete<Stats>(item);
                }
            });
        }
    }
}
