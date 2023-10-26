using Darts.Api.Extensions;
using Darts.Api.Mappers;
using Darts.Api.Services;
using Darts.Contract;
using Darts.Domain.Abstracts;
using Darts.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Darts.Api.Controllers
{
   [Authorize]
   [Route("api/[controller]")]
   [ApiController]
   public class RegistrationController : Controller
   {
      private readonly RequestRegistrationService _registrationService;
      private readonly AuthenticateService _authenticateService;
      private readonly IEmailService _emailService;

      public RegistrationController(RequestRegistrationService registrationService, AuthenticateService authenticateService, IEmailService emailService)
      {
         _registrationService = registrationService;
         _authenticateService = authenticateService;
         _emailService = emailService;
      }

      [AllowAnonymous]
      [HttpPost("create")]
      public IActionResult CreateRequestRegistrationAsync([FromBody] RequestRegistration requestRegistration)
      {
         var result = _registrationService.RequestRegistration(RequestRegistrationMapper.Map(requestRegistration));
         return Ok(result.ToContract(x => RequestRegistrationMapper.Map(x)));
      }

      [Authorize(Roles = Roles.SuperUserRole)]
      [HttpPost]
      public IActionResult EditRegistration([FromBody] RequestRegistration requestRegistration)
      {
         var result = _registrationService.Edit(RequestRegistrationMapper.Map(requestRegistration));
         return Ok(result.ToContract(x => RequestRegistrationMapper.Map(x)));
      }

      [Authorize(Roles = Roles.SuperUserRole)]
      [HttpPost("confirm/{id}")]
      public async Task<IActionResult> ConfirmRegistrationAsync(long id)
      {
         var result = _registrationService.Confirm(id);
         if(result.Validation == RequestRegistrationValidationResult.Success)
         {
            PasswordGenerator pg = new PasswordGenerator();
            var userPassword = pg.Generate();

            var registerResult = _authenticateService.Register(new Domain.DomainObjects.User
            {
               FirstName = result.Entity.FirstName,
               LastName = result.Entity.LastName,
               Email = result.Entity.Email,
               Password = userPassword
            });

            await _emailService.SendEmailAsync("matejputnok@gmail.com", result.Entity.Email, "Welcome to Darts world.", $"Welcome. Now you can login.\nYour password: {userPassword}");

            if (!registerResult)
            {
               return Conflict(new { message = "Email is already registered." });
            }
         }
         return Ok(result.ToContract(x => RequestRegistrationMapper.Map(x)));
      }

      [Authorize(Roles = Roles.SuperUserRole)]
      [HttpGet]
      public IActionResult GetRequestRegistrations([FromQuery] Filter pageInfo = null)
      {
         return Ok(_registrationService.GetRequestRegistrations(pageInfo));
      }

      [Authorize(Roles = Roles.SuperUserRole)]
      [HttpGet("{id}")]
      public IActionResult GetRequestRegistration(long id)
      {
         return Ok(_registrationService.Get(id));
      }
   }
}
