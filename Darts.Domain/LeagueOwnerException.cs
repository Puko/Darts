using System;

namespace Darts.Domain
{
   public class LeagueOwnerException : Exception
   {
      public LeagueOwnerException()
         : base("User that requested resource is not league owner")
      {

      }
   }
}
