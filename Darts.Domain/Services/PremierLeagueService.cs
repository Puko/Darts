using Darts.Contract;
using Darts.Domain.Abstracts.Factories;
using Darts.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using PremierLeague = Darts.Domain.DomainObjects.PremierLeague;

namespace Darts.Domain.Services
{
    public class PremierLeagueService : BaseService<PremierLeague>
    {
        public PremierLeagueService(IGenericContextFactory dbContextFactory) : base(dbContextFactory)
        {
        }

        public IEnumerable<PremierLeague> GetLeagues(long userId, Filter pageInfo, PremierLeagueInclusionInfo incusionInfo)
        {
            return GetAll(x => x.UserId == userId, include: x => Include(x, incusionInfo), x => x.OrderBy(pageInfo, l => l.Name), pageInfo);
        }

        public IEnumerable<PremierLeague> GetAll(PremierLeagueInclusionInfo incusionInfo, Filter pageInfo)
        {
            return GetAll(null, include: x => Include(x, incusionInfo), x => x.OrderBy(pageInfo, l => l.Name), pageInfo);
        }

        public PremierLeague GetLeague(long leagueId, PremierLeagueInclusionInfo incusionInfo)
        {
            var league = Get(x => x.Id == leagueId, include: x => Include(x, incusionInfo));
            return league;
        }

        public PremierLeague AddLeague(long userId, PremierLeague league)
        {
            Transaction(r =>
            {
                league.Created = DateTimeOffset.UtcNow;
                league.UserId = userId;

                r.Add(league);
            });

            return league;
        }

        public override PremierLeague Get(long id)
        {
            return Repository.GetSingle<PremierLeague>(x => x.Id == id);
        }

        private IIncludableQueryable<PremierLeague, object> Include(IQueryable<PremierLeague> x, PremierLeagueInclusionInfo inclusionInfo)
        {
            IIncludableQueryable<PremierLeague, object> result = x.Include(x => x.PremierLeaguePlayers);
            if (inclusionInfo.User)
            {
                result = result.Include(x => x.User);
            }
            if (inclusionInfo.Players)
            {
                result = result.Include(x => x.PremierLeaguePlayers).ThenInclude(x => x.Player);
            }

            return result;
        }

      public void Edit(long userId, long leagueId, PremierLeague edit)
      {
         
      }
   }
}
