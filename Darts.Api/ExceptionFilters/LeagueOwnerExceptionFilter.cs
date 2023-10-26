using System.Net;
using Darts.Domain;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Darts.Api.ExceptionFilters
{
   public class LeagueOwnerExceptionFilter : IExceptionFilter
   {
      public void OnException(ExceptionContext context)
      {
         if (context.Exception is LeagueOwnerException)
         {
            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Forbidden;
         }
      }
   }
}
