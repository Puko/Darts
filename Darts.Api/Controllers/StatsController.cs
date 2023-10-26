using System.Linq;
using Darts.Api.Extensions;
using Darts.Api.Mappers;
using Darts.Domain;
using Darts.Domain.DomainObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Darts.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : Controller
    {
        private readonly MatchService _matchService;
        private readonly StatsService _statsService;

        public StatsController(MatchService matchService, StatsService statsService)
        {
            _matchService = matchService;
            _statsService = statsService;
        }

        [AllowAnonymous]
        [HttpGet("{leagueId}/{matchId}")]
        public IActionResult GetStatsForMatch(long leagueId, long matchId)
        {
            var match = _matchService.GetMatch(leagueId, matchId, new MatchInclusionInfo
            {
                Players = true,
                Teams = true
            });

            Contract.MatchResult matchResult = new Contract.MatchResult();
            var homePlayers = match.HomeTeam.GetPlayers(leagueId).ToList();
            var guestPlayers = match.GuestTeam.GetPlayers(leagueId).ToList();

            var stats = match.Stats.ToList();

            foreach (var item in homePlayers)
            {
                var lmps = match.Stats.FirstOrDefault(x => x.PlayerId == item.Id);
                if (lmps != null)
                {
                    matchResult.HomeStats.Add(StatsMapper.Map(lmps));
                }
            }

            foreach (var item in guestPlayers)
            {
                var lmps = match.Stats.FirstOrDefault(x => x.PlayerId == item.Id);
                if (lmps != null)
                {
                    matchResult.GuestStats.Add(StatsMapper.Map(lmps));
                }
            }

            return Ok(matchResult);
        }

        [Authorize]
        [HttpPost("{leagueId}/{matchId}")]
        public IActionResult AddStats(long leagueId, long matchId, [FromBody] Contract.MatchResult matchResult)
        {
            var result = _statsService.Add(User.GetId(), leagueId, matchResult.GuestStats.Concat(matchResult.HomeStats).Select(x => StatsMapper.ToDomain(x)));

            return Ok(result.ToContract(x => x.Select(s => StatsMapper.Map(s))));
        }

        [Authorize]
        [HttpPatch("{leagueId}/{matchId}")]
        public IActionResult EditStats(long leagueId, long matchId, [FromBody] Contract.MatchResult matchResult)
        {
            var result = _statsService.Edit(User.GetId(), leagueId, matchId, matchResult.GuestStats.Concat(matchResult.HomeStats).Select(x => StatsMapper.ToDomain(x)));

            return Ok(result.ToContract(x => x.Select(s => StatsMapper.Map(s))));
        }

       

        [Authorize]
        [HttpDelete("{leagueId}/{matchId}")]
        public IActionResult DeleteStats(long leagueId, long matchId)
        {
            _statsService.Delete(User.GetId(), leagueId, matchId);

            return Ok();
        }
    }
}
