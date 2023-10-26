using Microsoft.EntityFrameworkCore;
using OfferingSolutions.GenericEFCore.RepositoryContext;

namespace Darts.Api.Data.Repositories
{
   public class GenericRepository<T> : GenericRepositoryContext<T> where T : class
   {
      public GenericRepository(DbContext databaseContext) : base(databaseContext)
      {
      }
   }
}