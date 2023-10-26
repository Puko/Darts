using System;
using System.Linq;
using System.Linq.Expressions;
using Darts.Contract;
using Darts.Domain.Abstracts.Factories;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Query;
using OfferingSolutions.GenericEFCore.UnitOfWorkContext;
using League = Darts.Domain.DomainObjects.League;
using PremierLeague = Darts.Domain.DomainObjects.PremierLeague;

namespace Darts.Domain
{
    public abstract class BaseService<T> where T : class
   {
      private readonly AbstractValidator<T> _validator;

      public BaseService(IGenericContextFactory dbContextFactory, AbstractValidator<T> validator = null)
      {
         Factory = dbContextFactory;
         _validator = validator;
      }

      private IGenericContextFactory Factory { get; }
      protected IOsUnitOfWorkContext Repository => Factory.UnitOfWork;
      protected virtual FluentValidation.Results.ValidationResult Validate(T toValidate)
      {
         if (_validator == null)
            return null;

         return _validator.Validate(toValidate);
      }

      protected void EnsureLeagueOwner(long userId, long leagueId)
      {
         if (Repository.GetSingle<League>(x => x.UserId == userId && x.Id == leagueId) == null 
            && Repository.GetSingle<PremierLeague>(x => x.UserId == userId && x.Id == leagueId) == null)
         {
            throw new LeagueOwnerException();
         }
      }

      public abstract T Get(long id);

      public T Get(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
      {
         return Repository.GetSingle(predicate, include: include);
      }

      public int Count()
      {
          return Repository.GetAll<T>().Count();
      }

      public int Count(Expression<Func<T, bool>> predicate)
      {
         return Repository.GetAll<T>().Count(predicate);
      }

      public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Filter pageInfo = null)
      {
         return GetAll(Repository, predicate, include, orderBy, pageInfo);
      }

      public IQueryable<T> GetAll(IOsUnitOfWorkContext context, Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Filter pageInfo = null)
      {
         return context.GetAll(predicate, include, orderBy, pageInfo?.Skip != null ? pageInfo.Skip : null, pageInfo?.PageSize != null ? pageInfo.PageSize : null);
      }

      public T Get(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include = null)
      {
         return Execute(r =>
         {
            var all = r.GetAll<T>();
            if (include != null)
            {
               all = include.Invoke(all);
            }

            return all.FirstOrDefault(predicate);
         });
      }

      public T Get(Expression<Func<T, bool>> predicate)
      {
         return Factory.UnitOfWork.GetSingle(predicate);
      }

      public void Delete(long userId, long leagueId, long objectId)
      {
         EnsureLeagueOwner(userId, leagueId);

         var toRemove = Get(objectId);
         Transaction(r => r.Delete(toRemove));
      }

      public void Add(long userId, long leagueId, T item)
      {
         EnsureLeagueOwner(userId, leagueId);
         Transaction(r => r.Add(item));
      }

      protected void Transaction(Action<IOsUnitOfWorkContext> action)
      {
         var repository = Factory.UnitOfWork;
         action.Invoke(repository);
         repository.Save();
      }

      protected TResult Transaction<TResult>(Func<IOsUnitOfWorkContext, TResult> action)
      {
         var repository = Factory.UnitOfWork;
         TResult result = action.Invoke(repository);
         repository.Save();

         return result;
      }

      protected TResult Execute<TResult>(Func<IOsUnitOfWorkContext, TResult> action)
      {
         var repository = Factory.UnitOfWork;
         TResult result = action.Invoke(repository);
      
         return result;
      }
   }
}
