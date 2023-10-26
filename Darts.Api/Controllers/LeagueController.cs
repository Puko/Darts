using Darts.Api.Attributes;
using Darts.Domain;
using Darts.Domain.DomainObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeagueIncusionInfo = Darts.Domain.DomainObjects.LeagueIncusionInfo;
using System.Linq;
using Darts.Api.Extensions;
using Darts.Api.Mappers;
using Darts.Contract;
using System.Collections.Generic;
using Stats = Darts.Contract.Stats;
using System;

namespace Darts.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : Controller
    {
        private readonly LeagueService _leagueService;
        private readonly TeamsService _teamsService;
        private readonly MatchService _matchService;
        private readonly StatsService _statsService;
        private readonly PlayerService _playerService;

        public LeagueController(LeagueService leagueService, TeamsService teamsService, MatchService matchService,
           StatsService statsService, PlayerService playerService)
        {
            _leagueService = leagueService;
            _teamsService = teamsService;
            _matchService = matchService;
            _statsService = statsService;
            _playerService = playerService;
        }

        #region Leagues
        /// <summary>
        /// Get concrete league
        /// </summary>
        /// <param name="leagueId">League id</param>
        [AllowAnonymous]
        [HttpGet("{leagueId}")]
        [SwaggerParameterPage]
        [SwaggerParameterPageSize]
        public IActionResult GetLeague([FromRoute] long leagueId)
        {
            var result = _leagueService.GetLeague(leagueId, new LeagueIncusionInfo { User = true });
            return Ok(LeagueMapper.Map(result));
        }

        /// <summary>
        /// Get all leagues
        /// </summary>
        /// <param name="pageInfo">Paging parameters</param>
        [AllowAnonymous]
        [HttpGet]
        [SwaggerParameterPage]
        [SwaggerParameterPageSize]
        public IActionResult GetLeagues([FromQuery] Filter pageInfo = null)
        {
            var result = _leagueService.GetLeagues(new LeagueIncusionInfo { User = true }, pageInfo);
            return Ok(result.ToContract(x => LeagueMapper.Map(x)));
        }

        /// <summary>
        /// Get all user leagues
        /// </summary>
        /// <param name="pageInfo">Paging parameters</param>
        [AllowAnonymous]
        [HttpGet("my")]
        [SwaggerParameterPage]
        [SwaggerParameterPageSize]
        public IActionResult GetUserLeagues([FromQuery] Filter pageInfo = null)
        {
            var result = _leagueService.GetLeagues(User.GetId(), pageInfo, new LeagueIncusionInfo { User = true });
            return Ok(result.ToContract(x => LeagueMapper.Map(x)));
        }

        /// <summary>
        /// Get all leagues in concrete year
        /// </summary>
        /// <param name="year">Paging parameters</param>
        [AllowAnonymous]
        [HttpGet("yearly/{year}")]
        [SwaggerParameterPage]
        [SwaggerParameterPageSize]
        public IActionResult GetLeagues([FromRoute] int year)
        {
            var result = _leagueService.GetLeagues(year, new LeagueIncusionInfo { User = true });
            return Ok(result.Select(x => LeagueMapper.Map(x)));
        }

        /// <summary>
        /// Edit league
        /// </summary>
        /// <param name="league">League object</param>
        [Authorize]
        [HttpPatch("{leagueId}")]
        [SwaggerParameterPage]
        [SwaggerParameterPageSize]
        public IActionResult EditLeague([FromBody] Contract.League league)
        {
            var leagueToEdit = LeagueMapper.Map(league);
            var result = _leagueService.Edit(User.GetId(), leagueToEdit);

            return Ok(result.ToContract(x => LeagueMapper.Map(leagueToEdit)));
        }

        /// <summary>
        /// Create league 
        /// </summary>
        /// <param name="league">League object</param>
        [Authorize]
        [HttpPost]
        [SwaggerParameterPage]
        [SwaggerParameterPageSize]
        public IActionResult CreateLeague([FromBody] Contract.League league)
        {
            var result = _leagueService.Add(User.GetId(), LeagueMapper.Map(league));

            return Ok(result.ToContract(x => LeagueMapper.Map(x)));
        }

        /// <summary>
        /// Delete league
        /// </summary>
        /// <param name="leagueId">League id</param>
        [Authorize]
        [HttpDelete("{leagueId}")]
        [SwaggerParameterPage]
        [SwaggerParameterPageSize]
        public IActionResult DeleteLeague([FromRoute] long leagueId)
        {
            _leagueService.DeleteLeague(User.GetId(), leagueId);
            return Ok();
        }
        #endregion

        #region Teams

        /// <summary>
        /// Get league teams
        /// </summary>
        /// <param name="leagueId">League id</param>
        [AllowAnonymous]
        [HttpGet("team/{leagueId}")]
        public IActionResult GetLeagueTeams(long leagueId)
        {
            var teams = _teamsService.GetTeams(leagueId, null, new TeamIncusionInfo
            {
                Matches = true
            });
            return Ok(teams);
        }

        /// <summary>
        /// Delete team from league
        /// </summary>
        /// <param name="leagueId">League id</param>
        /// <param name="teamId">Team id</param>
        [Authorize]
        [HttpDelete("team/{leagueId}/{teamId}")]
        public IActionResult DeleteTeam(long leagueId, long teamId)
        {
            _teamsService.DeleteTeamFromLeague(User.GetId(), leagueId, teamId);
            return Ok();
        }

        /// <summary>
        /// Add team to league
        /// </summary>
        /// <param name="leagueId">League id</param>
        /// <param name="teamId">Team id</param>
        [Authorize]
        [HttpPut("team/{leagueId}/{teamId}")]
        public IActionResult AssignTeam(long leagueId, long teamId)
        {
            var result = _teamsService.AssignTeam(teamId, leagueId);
            return Ok(result.ToContract(x => LeagueTeamMapper.Map(x)));
        }
        #endregion

        #region Matches
        /// <summary>
        /// Get all matches for league
        /// </summary>
        /// <param name="leagueId">League id</param>
        [AllowAnonymous]
        [HttpGet("match/{leagueId}")]
        public IActionResult GetMatchesForLeague(long leagueId)
        {
            var matches = _matchService.GetMatches(leagueId, new MatchInclusionInfo { Teams = true });
            var result = matches.Select(x => MatchMapper.Map(x));
            return Ok(result);
        }

        /// <summary>
        /// Get concrete match for league
        /// </summary>
        /// <param name="leagueId">League id</param>
        /// <param name="matchId">Match id</param>
        [AllowAnonymous]
        [HttpGet("match/{leagueId}/{matchId}")]
        public IActionResult GetMatchForLeague(long leagueId, long matchId)
        {
            var match = _matchService.GetMatch(leagueId, matchId, new MatchInclusionInfo { Players = true });
            var result = MatchMapper.Map(match);
            return Ok(result);
        }

        /// <summary>
        /// Get concrete match stats
        /// </summary>
        /// <param name="matchId">Match id</param>

        [AllowAnonymous]
        [HttpGet("match/stats/{matchId}")]
        public IActionResult GetMatchStats(long matchId)
        {
            var stats = _statsService.GetStats(matchId);
            var result = stats.Select(x => StatsMapper.Map(x));
            return Ok(result);
        }

        /// <summary>
        /// Get all league matches for concrete team
        /// </summary>
        /// <param name="leagueId">League id</param>
        /// <param name="teamId">Team id</param>
        [AllowAnonymous]
        [HttpGet("match/teams/{leagueId}/{teamId}")]
        public IActionResult GetMatchesForLeagueAndTeam(long leagueId, long teamId)
        {
            var matches = _matchService.GetMatches(leagueId, teamId, null, new MatchInclusionInfo { Teams = true }).OrderBy(x => x.Date);
            var result = matches.Select(x => MatchMapper.Map(x));
            return Ok(result);
        }

        /// <summary>
        /// Delete league match
        /// </summary>
        /// <param name="leagueId">League id</param>
        /// <param name="matchId">Match id</param>
        [Authorize]
        [HttpDelete("match/{leagueId}/{matchId}")]
        public IActionResult DeleteMatch(long leagueId, long matchId)
        {
            _matchService.Delete(User.GetId(), leagueId, matchId);
            return Ok();
        }

        /// <summary>
        /// Add league match
        /// </summary>
        /// <param name="match">Match object</param>
        [Authorize]
        [HttpPost("match")]
        public IActionResult AddMatch(AddEditMatch match)
        {
            var result = _matchService.Add(User.GetId(), Convert(null, match));
            if (result.Validation == MatchValidationResult.Success)
            {
                if (match.Result != null)
                {
                    _statsService.Add(User.GetId(), match.LeagueId, match.Result.GuestStats.Concat(match.Result.HomeStats).Select(x => StatsMapper.ToDomain(x)));
                }

            }

            return Ok(result.ToContract(x => MatchMapper.Map(x)));
        }

        /// <summary>
        /// Add league matches
        /// </summary>
        /// <param name="matches">Array of Match objects</param>
        [Authorize]
        [HttpPost("matches")]
        public IActionResult AddMatches(IEnumerable<AddEditMatch> matches)
        {
            var result = _matchService.AddMultiple(User.GetId(), matches.Select(x => Convert(null, x)));
            return Ok(result.Select(x => x.ToContract(x => MatchMapper.Map(x))));
        }

        /// <summary>
        /// Edit league match
        /// </summary>
        /// <param name="matchId">Match id</param>
        /// <param name="match">Match object</param>
        [Authorize]
        [HttpPatch("match/{matchId}")]
        public IActionResult EditMatch(long matchId, AddEditMatch match)
        {
            var result = _matchService.Edit(User.GetId(), Convert(matchId, match));
            if (result.Validation == MatchValidationResult.Success)
            {
                if (match.Result != null)
                {
                    if (match.Result.IsEmpty)
                    {
                        _statsService.Delete(User.GetId(), match.LeagueId, matchId);
                    }
                    else
                    {
                        _statsService.Edit(User.GetId(), match.LeagueId, matchId, match.Result.GuestStats.Concat(match.Result.HomeStats).Select(x => StatsMapper.ToDomain(x)));
                    }
                }

            }
            return Ok(result.ToContract(x => MatchMapper.Map(x)));
        }
        #endregion

        #region Stats
        /// <summary>
        /// Add stats for match
        /// </summary>
        /// <param name="leagueId">League id</param>
        /// <param name="matchId">Match id</param>
        /// <param name="stats">Stats id</param>
        [Authorize]
        [HttpPost("stats/{leagueId}/{matchId}")]
        public IActionResult AddStats(long leagueId, long matchId, [FromBody]Stats stats)
        {
            var result = _statsService.Add(User.GetId(), leagueId, StatsMapper.ToDomain(stats));
            return Ok(result.ToContract(x => StatsMapper.Map(x)));
        }

        /// <summary>
        /// Edit stats for match
        /// </summary>
        /// <param name="leagueId">League id</param>
        /// <param name="matchId">Match id</param>
        /// <param name="stats">Stats id</param>
        [Authorize]
        [HttpPatch("stats/{leagueId}/{matchId}")]
        public IActionResult EditStats(long leagueId, long matchId, [FromBody] Stats stats)
        {
            var result = _statsService.Edit(User.GetId(), leagueId, StatsMapper.ToDomain(stats));
            return Ok(result.ToContract(x => StatsMapper.Map(x)));
        }

        #endregion

        #region Players
        /// <summary>
        /// Add player to team in league
        /// </summary>
        /// <param name="playerId">Player id</param>
        /// <param name="leagueId">League id</param>
        /// <param name="teamId">Team id</param>
        [Authorize]
        [HttpPut("player/{leagueId}/{teamId}/{playerId}")]
        public IActionResult AssignPlayer(long playerId, long leagueId, long teamId)
        {
            var result = _playerService.AssignPlayer(playerId, teamId, leagueId);
            return Ok(result.ToContract(x => LeagueTeamPlayerMapper.Map(x)));
        }

        /// <summary>
        /// Delete player from team in league
        /// </summary>
        /// <param name="playerId">Player id</param>
        /// <param name="leagueId">League id</param>
        /// <param name="teamId">Team id</param>
        [Authorize]
        [HttpDelete("player/{leagueId}/{teamId}/{playerId}")]
        public IActionResult DeletePlayer(long leagueId, long teamId, long playerId)
        {
            _playerService.DeletePlayerFromLeagueAndTeam(User.GetId(), leagueId, teamId, playerId);
            return Ok();
        }

        /// <summary>
        /// Get league players
        /// </summary>
        /// <param name="leagueId">League id</param>
        /// <param name="filter">Filter object</param>
        [AllowAnonymous]
        [HttpGet("players/{leagueId}")]
        public IActionResult GetPlayersForLeague(long leagueId, [FromQuery] Filter filter)
        {
            var result = _playerService.GetPlayersForLeague(leagueId, filter, new PlayerInclusionInfo { Stats = true });
            return Ok(result.ToContract(x => PlayerMapper.Map(x)));
        }

        /// <summary>
        /// Get team players in league
        /// </summary>
        /// <param name="leagueId">League id</param>
        /// <param name="teamId">Team id</param>
        [AllowAnonymous]
        [HttpGet("players/{leagueId}/{teamId}")]
        public IActionResult GetPlayersForLeagueAndTeam(long leagueId, long teamId)
        {
            var result = _playerService.GetPlayers(teamId, leagueId, null, new PlayerInclusionInfo { Stats = true });
            var players = result.Select(x => PlayerMapper.Map(x));
            return Ok(players);
        }
        #endregion

        #region Table
        [AllowAnonymous]
        [HttpGet("table/{leagueId}")]
        public IActionResult GetTable(long leagueId)
        {
            var result = _teamsService.GetTeams(leagueId, null, new TeamIncusionInfo { Matches = true }, null);
            var teams = result.Select(x => TeamMapper.Map(x)).ToList();

            return Ok(teams);
        }
        #endregion

        //[AllowAnonymous]
        //[HttpGet("{matchId}")]
        //public IActionResult GetImage(long matchId)
        //{
        //   var image = _matchService.GetEnrollment(matchId);
        //   return File(image ?? new byte[0], "image/png");
        //}

        private static Domain.DomainObjects.Match Convert(long? id, AddEditMatch match)
        {
            var matchToEdit = new Domain.DomainObjects.Match
            {
                Id = id ?? 0,
                LeagueId = match.LeagueId,
                HomeTeamId = match.HomeTeamId,
                GuestTeamId = match.GuestTeamId,
                IsOvertime = match.IsOvertime,
                Date = match.Date
            };

            if (match.Result != null)
            {
                matchToEdit.HomePoints = match.Result.HomeStats.Sum(x => x.Points);
                matchToEdit.HomeLegs = match.Result.HomeStats.Sum(x => x.WinLegs);
                matchToEdit.GuestPoints = match.Result.GuestStats.Sum(x => x.Points);
                matchToEdit.GuestLegs = match.Result.GuestStats.Sum(x => x.WinLegs);
            }

            return matchToEdit;
        }
    }
}
