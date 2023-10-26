using Darts.Api.Extensions;
using Darts.Api.Mappers;
using Darts.Api.Services;
using Darts.Contract;
using Darts.Domain.DomainObjects;
using Darts.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using ChangePassword = Darts.Domain.DomainObjects.ChangePassword;

namespace Darts.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUser()
        {
            IActionResult result = BadRequest();
            var user = _userService.Get(User.GetId());
            if(user != null)
            {
                result = Ok(UserMapper.Map(user));
            }
            return result;
        }

        [Authorize(Roles = Roles.SuperUserRole)]
        [HttpGet("all")]
        public IActionResult GetUsers([FromQuery] Filter filter)
        {
            IActionResult result = BadRequest();
            var claim = User.FindFirst(ClaimTypes.Sid);
            if (claim != null)
            {
                var pageResult = _userService.GetAll(filter);
                result = Ok(pageResult.ToContract(x => UserMapper.Map(x)));
            }

            return result;
        }

        [Authorize]
        [HttpPatch]
        public IActionResult UpdateUser([FromBody] AddEditUser user)
        {
            IActionResult result = Forbid();
            var idFromToken = User.GetId();
            var usr = _userService.Get(idFromToken);

            if(usr != null)
            {
                usr.Email = user.Email;
                usr.FirstName = user.FirstName;
                usr.LastName = user.LastName;
                usr.Mobile = user.Mobile;

                _userService.Update(usr);
            }

            return Ok();
        }

        [Authorize(Roles = Roles.SuperUserRole)]
        [HttpPatch("{id}")]
        public IActionResult ActivateDeactivateUser(long id, [FromQuery] bool active)
        {
            IActionResult result = BadRequest();
            var usr = _userService.Get(id);

            if (usr != null)
            {
                usr.IsActive = active;
                _userService.Update(usr);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("changePassword")]
        public IActionResult ChangeUserPassword([FromBody] ChangePassword changePassword)
        {
            if(string.IsNullOrEmpty(changePassword.NewPassword) || string.IsNullOrEmpty(changePassword.OldPassword))
            {
                return BadRequest();
            }

            var result = _userService.ChangePassword(User.GetId(), changePassword);

            if (result)
                return Ok();
            else
                return BadRequest();
        }
    }
}
