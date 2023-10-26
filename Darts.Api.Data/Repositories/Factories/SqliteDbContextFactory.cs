using System;
using Darts.Domain.Abstracts.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OfferingSolutions.GenericEFCore.UnitOfWorkContext;

namespace Darts.Api.Data.Repositories.Factories
{
   public class SqliteDbContextFactory : IGenericContextFactory
   {
      private readonly string _connectionString;

      public SqliteDbContextFactory(string connectionString = "Filename=DartsDatabase.db")
      {
         _connectionString = connectionString;
      }

      public IOsUnitOfWorkContext UnitOfWork => new OsUnitOfWorkContext(new DartsContext(new DbContextOptionsBuilder().UseSqlite(_connectionString).Options));
   }
}
