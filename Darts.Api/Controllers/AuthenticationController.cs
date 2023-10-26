using Darts.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Darts.Contract;

namespace Darts.Api.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class AuthenticationController : Controller
   {
      private readonly AuthenticateService _authenticateService;

      public AuthenticationController(AuthenticateService authenticateService)
      {
         _authenticateService = authenticateService;
      }

      [AllowAnonymous]
      [HttpPost("login")]
      public IActionResult Login([FromBody] Login login)
      {
         JwtToken token = _authenticateService.Authenticate(login.Email, login.Password);
         if (token == null)
         {
            return BadRequest(new { message = "Bad username or password." });
         }

         return Ok(token);
      }

      //[AllowAnonymous]
      //[HttpPost("register")]
      //public IActionResult Register([FromBody] Contract.User user)
      //{
      //    var result = _authenticateService.Register(new Domain.DomainObjects.User 
      //    {
      //        Created = user.Created,
      //        Email = user.Email,
      //        FirstName = user.FirstName,
      //        Id = user.Id,   
      //        IsActive = true,
      //        LastName = user.LastName,
      //        Mobile = user.Mobile,
      //        Password = user.Password,
      //        UserRole = Domain.DomainObjects.UserRole.User
      //    });

      //    if (!result)
      //    {
      //        return Conflict(new { message = "Email is already registered." });
      //    }

      //    return Ok();
      //}
   }
}
