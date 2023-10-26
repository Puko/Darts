using Darts.Contract;
using Darts.Domain;
using Darts.Domain.DomainObjects;
using Darts.Domain.Services;
using NUnit.Framework;
using System;
using System.Linq;

namespace Darts.Tests
{
   public class LeagueTests : BaseTest
   {
      [Test]
      public void League_GetConcreteLeague_Success()
      {
         //arrange 
         var leagueService = Resolve<LeagueService>();
         var league = CreateLeague(userId: User.Id);
         var result = leagueService.Add(User.Id, league);

         //act
         var newLeague = leagueService.GetLeague(result.Entity.Id, new LeagueIncusionInfo { User = true });

         //assert
         Assert.NotNull(newLeague);
         Assert.NotNull(newLeague.User);
      }

      [Test]
      public void League_GetPaging_Success()
      {
         //arrange 
         var leagueService = Resolve<LeagueService>();
         for (int i = 0; i < 100; i++)
         {
            var league = CreateLeague(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), userId: User.Id);
            var result = leagueService.Add(User.Id, league);
         }
         
         //act
         var pagedResult = leagueService.GetLeagues(User.Id, new Filter
         {
            PageNumber = 1,
            PageSize = 10
         }, new LeagueIncusionInfo { User = true });

         //assert
         Assert.NotNull(pagedResult);
         Assert.AreEqual(100, pagedResult.Count);
         Assert.AreEqual(10, pagedResult.Items.Count());
      }

      [Test]
      public void League_GetPagingFoConcreteUser_Success()
      {
         //arrange 
         var user = CreateUser(Resolve<UserService>(), "123123123", "asdadasd", "zxzxcas");
         var leagueService = Resolve<LeagueService>();

         for (int i = 0; i < 50; i++)
         {
            var league = CreateLeague(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), userId: User.Id);
            var result = leagueService.Add(User.Id, league);
         }

         for (int i = 0; i < 50; i++)
         {
            var league = CreateLeague(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), userId: user.Id);
            var result = leagueService.Add(user.Id, league);
         }

         //act
         var pagedResult = leagueService.GetLeagues(user.Id, new Filter
         {
            PageNumber = 1,
            PageSize = 100
         }, new LeagueIncusionInfo { User = true });

         //assert
         Assert.NotNull(pagedResult);
         Assert.AreEqual(50, pagedResult.Count);
         Assert.AreEqual(50, pagedResult.Items.Count());
      }

      [Test]
      public void League_Add_Success()
      {
         //arrange 
         var leagueService = Resolve<LeagueService>();
         var league = CreateLeague(userId: User.Id);

         //act
         var result = leagueService.Add(User.Id, league);

         //assert
         Assert.AreEqual(LeagueValidationResult.Success, result.Validation);
      }

      [Test]
      public void League_Add_MandatoryFieldsNotFilled()
      {
         //arrange 
         var leagueService = Resolve<LeagueService>();
         var league = CreateLeague(null, userId: User.Id);

         //act
         var result = leagueService.Add(User.Id, league);

         //assert
         Assert.AreEqual(LeagueValidationResult.MandatoryFieldsNotFilled, result.Validation);
      }

      [Test]
      public void League_Add_LeagueWithNameAlreadyExist()
      {
         //arrange 
         var leagueService = Resolve<LeagueService>();
         var league = CreateLeague("League1", userId: User.Id);
         var result = leagueService.Add(User.Id, league);

         //act
         var league1 = CreateLeague("League1", userId: User.Id);
         var result1 = leagueService.Add(User.Id, league1);
         
         //assert
         Assert.AreEqual(LeagueValidationResult.LeagueWithNameAlreadyExist, result1.Validation);
      }

      [Test]
      public void League_Edit_Success()
      {
         //arrange 
         var leagueService = Resolve<LeagueService>();
         var league = CreateLeague("League1", userId: User.Id);
         var result = leagueService.Add(User.Id, league);

         //act
         var edit = result.Entity;
         edit.Name = "Edit";
         var result1 = leagueService.Edit(User.Id, edit);

         //assert
         Assert.AreEqual(LeagueValidationResult.Success, result1.Validation);
         Assert.AreEqual("Edit", result1.Entity.Name);
      }

      [Test]
      public void League_Edit_MandatoryFieldsNotFilled()
      {
         //arrange 
         var leagueService = Resolve<LeagueService>();
         var league = CreateLeague("League1", userId: User.Id);
         var result = leagueService.Add(User.Id, league);

         //act
         var edit = result.Entity;
         edit.Name = null;
         var result1 = leagueService.Edit(User.Id, edit);

         //assert
         Assert.AreEqual(LeagueValidationResult.MandatoryFieldsNotFilled, result1.Validation);
      }

      [Test]
      public void League_Edit_LeagueWithNameAlreadyExist()
      {
         //arrange 
         var leagueService = Resolve<LeagueService>();
         var league = CreateLeague("League1", userId: User.Id);
         var result = leagueService.Add(User.Id, league);

         //act
         var edit = result.Entity;
         edit.Name = "League1";
         var result1 = leagueService.Edit(User.Id, edit);

         //assert
         Assert.AreEqual(LeagueValidationResult.LeagueWithNameAlreadyExist, result1.Validation);
      }

      public static Domain.DomainObjects.League CreateLeague(string name = "Test league", string shortCut = "tst", int pointsPerWin = 3, long userId = 0)
      {
         return new Domain.DomainObjects.League
         {
            UserId = userId,
            Created = DateTimeOffset.Now,
            Name = name,
            ShortCut = shortCut,
            PointsPerWin = 3
         };
      }
   }
}
