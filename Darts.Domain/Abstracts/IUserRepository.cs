using Darts.Domain.DomainObjects;
using OfferingSolutions.GenericEFCore.RepositoryContext;

namespace Darts.Domain.Abstracts
{
   public interface IUserRepository : IGenericRepositoryContext<User>
   {
      User GetUser(string email);
   }
}
