using Darts.Api.Data;
using Darts.Domain.Abstracts.Factories;
using OfferingSolutions.GenericEFCore.UnitOfWorkContext;

namespace Darts.Tests
{
   internal class InMemoryDbContextFactory : IGenericContextFactory
   {
      private readonly DartsContext _dartsContext;

      public InMemoryDbContextFactory(DartsContext dartsContext)
      {
         _dartsContext = dartsContext;
      }

      public IOsUnitOfWorkContext UnitOfWork => new OsUnitOfWorkContext(_dartsContext);
   }
}
