using System.Linq;
using Darts.Api.Extensions;
using Darts.Api.Mappers;
using Darts.Contract;
using Darts.Domain;
using Darts.Domain.DomainObjects;
using Darts.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeagueTeamPlayer = Darts.Contract.LeagueTeamPlayer;
using Player = Darts.Contract.Player;

namespace Darts.Api.Controllers
{
    [Authorize]
   [Route("api/[controller]")]
   [ApiController]
   public class PlayerController : Controller
   {
      private readonly PlayerService _playerService;


      public PlayerController(PlayerService playerService)
      {
         _playerService = playerService;
      }

      [AllowAnonymous]
      [HttpGet("player/{playerId}")]
      public IActionResult GetPlayer(long playerId)
      {
         var result = _playerService.GetPlayer(playerId, new PlayerInclusionInfo { User = true });
         return Ok(PlayerMapper.Map(result));
      }

      [AllowAnonymous]
      [HttpGet]
      public IActionResult GetAllPlayers([FromQuery] Filter filter)
      {
         var result = _playerService.GetAllPlayers(filter, new PlayerInclusionInfo { User = true });
         return Ok(result.ToContract(x => PlayerMapper.Map(x)));
      }

      [Authorize]
      [HttpPost]
      public IActionResult CreatePlayer(AddEditPlayer player)
      {
         var result = _playerService.Add(User.GetId(), new Domain.DomainObjects.Player { FirstName = player.FirstName, LastName = player.LastName, Identifier = player.Identifier });
         return Ok(result.ToContract(x => PlayerMapper.Map(x)));
      }

      [Authorize]
      [HttpPatch("{id}")]
      public IActionResult EditPlayer(long id, AddEditPlayer player)
      {
         var result = _playerService.Edit(User.GetId(), new Domain.DomainObjects.Player { Id = id, FirstName = player.FirstName, LastName = player.LastName, Identifier = player.Identifier });
         return Ok(result.ToContract(x => PlayerMapper.Map(x)));
      }

   }
}
