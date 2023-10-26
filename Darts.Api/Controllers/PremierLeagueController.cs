using Darts.Api.Attributes;
using Darts.Api.Extensions;
using Darts.Api.Mappers;
using Darts.Contract;
using Darts.Domain.DomainObjects;
using Darts.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using PremierLeague = Darts.Domain.DomainObjects.PremierLeague;

namespace Darts.Api.Controllers
{
    [Authorize]
   [Route("api/[controller]")]
   [ApiController]
   public class PremierLeagueController : ControllerBase
   {
      private readonly PremierLeagueService _premierLeagueService;
      private readonly PremierLeaguePlayerService _premierLeaguePlayerService;
      private readonly PremierLeagueMatchService _premierLeagueMatchService;

      public PremierLeagueController(PremierLeagueService premierLeagueService, PremierLeaguePlayerService premierLeaguePlayerService,
          PremierLeagueMatchService premierLeagueMatchService)
      {
         _premierLeagueService = premierLeagueService;
         _premierLeaguePlayerService = premierLeaguePlayerService;
         _premierLeagueMatchService = premierLeagueMatchService;
      }

#region Leagues
      /// <summary>
      /// Get all premier leagues
      /// </summary>
      /// <param name="pageInfo">Paging parameters</param>
      [AllowAnonymous]
      [HttpGet]
      [SwaggerParameterPage]
      [SwaggerParameterPageSize]
      public IActionResult GetPremierLeagues([FromQuery] Filter pageInfo = null)
      {
         var result = _premierLeagueService.GetAll(new PremierLeagueInclusionInfo { User = true }, pageInfo);
         var leauges = result.Select(x => PremierLeagueMapper.Map(x)).ToList();

         return Ok(leauges);
      }

      /// <summary>
      /// Get concrete league
      /// </summary>
      /// <param name="leagueId">League id</param>
      [AllowAnonymous]
      [HttpGet("{leagueId}")]
      [SwaggerParameterPage]
      [SwaggerParameterPageSize]
      public IActionResult GetPremierLeague([FromRoute] long leagueId)
      {
         var result = _premierLeagueService.GetLeague(leagueId, new PremierLeagueInclusionInfo { User = true });
         return Ok(PremierLeagueMapper.Map(result));
      }

      /// <summary>
      /// Edit league
      /// </summary>
      /// <param name="league">PremierLeague object</param>     
      /// <param name="leagueId">League id</param>
      [Authorize]
      [HttpPatch("{leagueId}")]
      [SwaggerParameterPage]
      [SwaggerParameterPageSize]
      public IActionResult EditPremierLeague([FromRoute] long leagueId, [FromBody] Contract.PremierLeague league)
      {
         var edit = new PremierLeague
         {
            Id = leagueId,
            Created = league.Created,
            UserId = league.UserId,
            Name = league.Name,
            ShortCut = league.ShortCut,
            PointsPerWin = league.PointsPerWin,
            PointsPerLoose = league.PointsPerLoose,
            PointsForDraw = league.PointsForDraw,
         };

         _premierLeagueService.Edit(User.GetId(), leagueId, edit);

         return Ok(PremierLeagueMapper.Map(edit));
      }

      /// <summary>
      /// Create league 
      /// </summary>
      /// <param name="league">PremierLeague object</param>
      [Authorize]
      [HttpPost]
      [SwaggerParameterPage]
      [SwaggerParameterPageSize]
      public IActionResult CreatePremierLeague([FromBody] Contract.PremierLeague league)
      {
         var result = _premierLeagueService.AddLeague(User.GetId(), new PremierLeague
         {
            Name = league.Name,
            ShortCut = league.ShortCut,
            PointsPerWin = league.PointsPerWin,
            PointsPerLoose = league.PointsPerLoose,
            PointsForDraw = league.PointsForDraw,
         });

         return Ok(PremierLeagueMapper.Map(result));
      }

      /// <summary>
      /// Delete league
      /// </summary>
      /// <param name="leagueId">League id</param>
      [Authorize]
      [HttpDelete("{leagueId}")]
      [SwaggerParameterPage]
      [SwaggerParameterPageSize]
      public IActionResult DeletePremierLeague([FromRoute] long leagueId)
      {
         _premierLeagueService.Delete(User.GetId(), leagueId, leagueId);
         return Ok();
      }
      #endregion

#region Players
      /// <summary>
      /// Get all league players 
      /// </summary>
      /// <param name="leagueId">League id</param>
      [AllowAnonymous]
      [HttpGet("players/{leagueId}")]
      public IActionResult GetPlayersForPremierLeague(long leagueId)
      {
         var result = _premierLeaguePlayerService.GetPlayersForPremierLeague(leagueId, null, new PlayerInclusionInfo { PremierLeagueMatches = true });
         var players = result.Select(x => PlayerMapper.Map(x));
         return Ok(players);
      }

      /// <summary>
      /// Add player to team in league
      /// </summary>
      /// <param name="playerId">Player id</param>
      /// <param name="leagueId">League id</param>
      [Authorize]
      [HttpPut("assign/{premierLeagueId}/{playerId}")]
      public IActionResult AssignPlayer(long leagueId, long playerId)
      {
         var result = _premierLeaguePlayerService.AssignPlayer(playerId, leagueId);
         return Ok(result.ToContract(x => PremierLeaguePlayerMapper.Map(x)));
      }

      /// <summary>
      /// Delete player from team in league
      /// </summary>
      /// <param name="playerId">Player id</param>
      /// <param name="leagueId">League id</param>
      [Authorize]
      [HttpDelete("{premierLeagueId}/{playerId}")]
      public IActionResult DeletePlayer(long leagueId, long playerId)
      {
         _premierLeaguePlayerService.DeletePremierPlayer(User.GetId(), leagueId, playerId);
         return Ok();
      }
      #endregion

#region Matches

      /// <summary>
      /// Get all player matches
      /// </summary>
      /// <param name="leagueId">League id</param>
      /// <param name="playerId">Player id</param>
      [AllowAnonymous]
      [HttpGet("match/{leagueId}/{playerId}")]
      public IActionResult GetPlayersMatchesForPremierLeague(long leagueId, long playerId)
      {
         var matches = _premierLeagueMatchService.GetPlayerMatches(leagueId, playerId).OrderBy(x => x.Date);
         var result = matches.Select(x => PremierLeagueMatchMapper.Map(x));
         return Ok(result);
      }

      /// <summary>
      /// Get all league matches
      /// </summary>
      /// <param name="leagueId">League id</param>
      [AllowAnonymous]
      [HttpGet("match/{leagueId}")]
      public IActionResult GetPremierLeagueMatchesForLeague(long leagueId)
      {
         var matches = _premierLeagueMatchService.GetMatches(leagueId);
         var result = matches.Select(x => PremierLeagueMatchMapper.Map(x));
         return Ok(result);
      }

      /// <summary>
      /// Delete match from league
      /// </summary>
      /// <param name="leagueId">League id</param>
      /// <param name="matchId">Match id</param>
      [Authorize]
      [HttpDelete("match/{leagueId}/{matchId}")]
      public IActionResult DeleteMatch(long leagueId, long matchId)
      {
         _premierLeagueMatchService.Delete(User.GetId(), leagueId, matchId);
         return Ok();
      }

      /// <summary>
      /// Add premier league match
      /// </summary>
      /// <param name="match">AddEditPremierLeagueMatch object</param>
      [Authorize]
      [HttpPost("match")]
      public IActionResult AddMatch(AddEditPremierLeagueMatch match)
      {
         var result = _premierLeagueMatchService.Add(User.GetId(), Convert(null, match));
         return Ok(result.ToContract(x => PremierLeagueMatchMapper.Map(x)));
      }

      /// <summary>
      /// Add premier league match
      /// </summary>
      /// <param name="match">AddEditPremierLeagueMatch object</param>
      /// <param name="matchId">Match id</param>
      [Authorize]
      [HttpPatch("match/{id}")]
      public IActionResult EditMatch(long matchId, AddEditPremierLeagueMatch match)
      {
         var result = _premierLeagueMatchService.Edit(User.GetId(), Convert(matchId, match));
         return Ok(result.ToContract(x => PremierLeagueMatchMapper.Map(x)));
      }
#endregion

      private static Domain.DomainObjects.PremierLeagueMatch Convert(long? id, AddEditPremierLeagueMatch match)
      {
         var matchToEdit = new Domain.DomainObjects.PremierLeagueMatch
         {
            Id = id ?? 0,
            PremierLeagueId = match.PremierLeagueId,
            Date = match.Date,
            GuestLegs = match.GuestLegs,
            GuestPlayerId = match.GuestPlayerId,
            GuestAverage = match.GuestAverage,
            HomeLegs = match.HomeLegs,
            HomePlayerId = match.HomePlayerId,
            HomeAverage = match.HomeAverage
         };

         return matchToEdit;
      }
   }
}
