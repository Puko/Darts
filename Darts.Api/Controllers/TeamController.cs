using Darts.Domain;
using Darts.Domain.DomainObjects;
using Darts.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Darts.Contract;
using Team = Darts.Domain.DomainObjects.Team;
using Darts.Api.Mappers;
using System.Linq;

namespace Darts.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : Controller
    {
        private readonly TeamsService _teamsService;
        private readonly LeagueService _leagueService;

        public TeamController(TeamsService teamsService, LeagueService leagueService)
        {
            _teamsService = teamsService;
            _leagueService = leagueService;
        }

        /// <summary>
        /// Get all teams
        /// </summary>
        /// <param name="filter">Filter object</param>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAllTeams([FromQuery] Filter filter)
        {
            var result = _teamsService.GetAllTeams(filter, TeamIncusionInfo.Empty);
            return Ok(result.ToContract(x => TeamMapper.Map(x)));
        }

        /// <summary>
        /// Get all leagues for team
        /// </summary>
        /// <param name="id">Team id</param>
        [AllowAnonymous]
        [HttpGet("leagues/{id}")]
        public IActionResult GetLeaguesForTeam(long id)
        {
            var leagues = _leagueService.GetLeagues(id);
            return Ok(leagues.Select(x => LeagueMapper.Map(x)));
        }

        /// <summary>
        /// Get concrete team
        /// </summary>
        /// <param name="teamId">Team id</param>
        [AllowAnonymous]
        [HttpGet("{teamId}")]
        public IActionResult GetTeam(long teamId)
        {
            var result = _teamsService.GetTeam(teamId, TeamIncusionInfo.Empty);
            return Ok(TeamMapper.Map(result));
        }

        /// <summary>
        /// Create team
        /// </summary>
        /// <param name="team">AddEditTeam object</param>
        [Authorize]
        [HttpPost]
        public IActionResult CreateTeam(AddEditTeam team)
        {
            var result = _teamsService.Add(new Team { Name = team.Name, Address = team.Address, City = team.City });
            return Ok(result);
        }

        /// <summary>
        /// Edit team
        /// </summary>
        /// <param name="team">AddEditTeam object</param>
        /// <param name="teamId">Team id</param>
        [Authorize]
        [HttpPatch("{teamId}")]
        public IActionResult EditTeam(long teamId, AddEditTeam team)
        {
            var result = _teamsService.Edit(User.GetId(), new Team { Id = teamId, Name = team.Name, Address = team.Address, City = team.City });
            return Ok(result);
        }
    }
}
