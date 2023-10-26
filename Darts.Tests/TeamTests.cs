using Darts.Domain.DomainObjects;

namespace Darts.Tests
{
   public class TeamTests : BaseTest
   {
      public static Team CreateTeam(string name = "test", string address = "address", string city = "city")
      {
         return new Team
         {
            Name = name,
            Address = address,
            City = city,
         };
      }
   }
}
