using OfferingSolutions.GenericEFCore.UnitOfWorkContext;

namespace Darts.Domain.Abstracts.Factories
{
   public interface IGenericContextFactory
   {
      public IOsUnitOfWorkContext UnitOfWork { get; }
   }
}
