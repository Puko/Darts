using Darts.Domain.Abstracts;
using Darts.Domain.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace Darts.Api.Data.Repositories
{
   public class RequestRegistrationRepository : GenericRepository<RequestRegistration>, IRequestRegistrationRepository
   {
      public RequestRegistrationRepository(DartsContext databaseContext) : base(databaseContext)
      {
      }
   }
}
