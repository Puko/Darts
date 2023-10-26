using Darts.Domain.DomainObjects;

namespace Darts.Domain.Csv
{
   public class CsvPlayer
   {
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string Identifier { get; set; }

      public Player Create()
      {
         return new Player
         {
            FirstName = FirstName,
            LastName = LastName,
            Identifier = Identifier
         };
      }
   }
}
