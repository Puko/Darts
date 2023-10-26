using Darts.Domain.DomainObjects;

namespace Darts.Domain.Csv
{
   public class CsvTeam
   {
      public string Name { get; set; }
      public string Address { get; set; }
      public string City { get; set; }

      public Team Create()
      {
         return new Team
         {
            City = City,
            Address = Address,
            Name = Name
         };
      }
   }
}
