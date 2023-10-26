using Darts.Api.Data;
using Darts.Api.Data.Repositories;
using Darts.Domain;
using Darts.Domain.Abstracts;
using Darts.Domain.Abstracts.Factories;
using Darts.Domain.DomainObjects;
using Darts.Domain.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimpleInjector;
using System;

namespace Darts.Tests
{
   public class BaseTest
   {
      private Container? _container;
      private Scope? _scope;

      protected User User { get; private set; }

      [SetUp]
      public void Setup()
      {
         _container = new Container();
         _container.Options.DefaultScopedLifestyle = ScopedLifestyle.Flowing;
         _container.Options.DefaultLifestyle = Lifestyle.Scoped;

         var context = new DartsContext(new DbContextOptionsBuilder().UseInMemoryDatabase("dartsTestDb").Options);
         _container.Register<DartsContext>(() => context);

         _container.Register<IGenericContextFactory, InMemoryDbContextFactory>();

         _container.Register<IUserRepository, UserRepository>();
         _container.Register<ILeagueRepository, LeagueRepository>();
         _container.Register<ITeamsRepository, TeamsRepository>();
         _container.Register<IPlayerRepository, PlayerRepository>();
         _container.Register<IMatchRepository, MatchRepository>();
         _container.Register<IStatsRepository, StatsRepository>();
         _container.Register<ILeagueTeamRepository, LeagueTeamRepository>();
         _container.Register<ILeagueTeamPlayerRepository, LeagueTeamPlayerRepository>();
         _container.Register<IPremierLeaguePlayerRepository, PremierLeaguePlayerRepository>();
         _container.Register<IPremierLeagueMatchRepository, PremierLeagueMatchRepository>();

         _container.Register<TeamsService>();
         _container.Register<LeagueService>();
         _container.Register<PlayerService>();
         _container.Register<MatchService>();
         _container.Register<StatsService>();
         _container.Register<PremierLeagueService>();
         _container.Register<PremierLeagueMatchService>();
         _container.Register<UserService>();
         _container.Register<PremierLeaguePlayerService>();

         _scope = new Scope(_container);

         AddUser();
      }

      [TearDown]
      public void TearDown()
      {
         _scope?.Dispose();
         _scope = null;

         _container?.Dispose();
         _container = null;
      }

      protected void AddUser()
      {
         var userService = Resolve<UserService>();
         User = CreateUser(userService);
      }

      protected User CreateUser(UserService userService, string email = "mp@gmail.com", string firstName = "Test", string lastName = "User")
      {
         return userService.Add(new User
         {
            Created = DateTime.Now,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            IsActive = true,
            Mobile = Guid.NewGuid().ToString(),
            Password = "test"
         });
      }

      protected T Resolve<T>() where T : class
      {
         if (_scope == null)
            throw new InvalidOperationException("Test isn't running in scope.");

         return _scope.GetInstance<T>();
      }
   }
}