using System;
using Darts.Domain.Abstracts.Factories;
using Microsoft.EntityFrameworkCore;
using OfferingSolutions.GenericEFCore.UnitOfWorkContext;

namespace Darts.Api.Data.Repositories.Factories
{
   public class MssqlDbContextFactory : IGenericContextFactory
   {
      private readonly string _connectionString;

      public MssqlDbContextFactory(string connectionString)
      {
         _connectionString = connectionString;
      }

      public IOsUnitOfWorkContext UnitOfWork => new OsUnitOfWorkContext(new DartsContext(new DbContextOptionsBuilder().UseSqlServer(_connectionString).Options));
   }
}
