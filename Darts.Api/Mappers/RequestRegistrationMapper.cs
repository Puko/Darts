using Darts.Contract;

namespace Darts.Api.Mappers
{
   public class RequestRegistrationMapper
   {
      public static Domain.DomainObjects.RequestRegistration Map(RequestRegistration match)
      {
         return Mapper.From<RequestRegistration, Domain.DomainObjects.RequestRegistration> (match);
      }

      public static RequestRegistration Map(Domain.DomainObjects.RequestRegistration match)
      {
         return Mapper.From<Domain.DomainObjects.RequestRegistration, RequestRegistration>(match);
      }
   }
}
