using System.Collections.Generic;
using Darts.Domain.DomainObjects;
using OfferingSolutions.GenericEFCore.RepositoryContext;

namespace Darts.Domain.Abstracts
{
   public interface ITeamsRepository : IGenericRepositoryContext<Team>
   {
   }
}
