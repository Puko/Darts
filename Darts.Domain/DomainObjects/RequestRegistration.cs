using System;

namespace Darts.Domain.DomainObjects
{
   public class RequestRegistration
   {
      public long Id { get; set; }
      public DateTimeOffset Created { get; set; }
      public string Email { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string Message { get; set; }
      public DateTimeOffset? Processed { get; set; }
      public string BanReason { get; set; }
   }
}
