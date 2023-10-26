using System.Linq;
using Darts.Domain.Abstracts;
using Darts.Domain.DomainObjects;

namespace Darts.Api.Data.Repositories
{
   public class UserRepository : GenericRepository<User>, IUserRepository
   {
      public UserRepository(DartsContext dartsContext)
         : base(dartsContext)
      {
      }

      public User GetUser(string email)
      {
         var userDto = GetSingle(x => x.Email.Equals(email));
         return userDto;
      }
   }
}
