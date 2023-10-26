using System;
using Darts.Domain.Abstracts.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OfferingSolutions.GenericEFCore.UnitOfWorkContext;

namespace Darts.Api.Data.Repositories.Factories
{
   public class PostgresDbContextFactory : IGenericContextFactory
   {
      private readonly string _connectionString;

      public PostgresDbContextFactory(string connectionString)
      {
         _connectionString = connectionString;
      }

      public IOsUnitOfWorkContext UnitOfWork => new OsUnitOfWorkContext(new DartsContext(new DbContextOptionsBuilder().UseNpgsql(_connectionString, options => options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), new System.Collections.Generic.List<string>())).Options));
   }
}
