using Darts.Domain.DomainObjects;
using FluentValidation;

namespace Darts.Domain.Validation
{
   public class TeamValidator : AbstractValidator<Team>
   {
      public TeamValidator()
      {
         RuleFor(x => x.Address).NotEmpty();
         RuleFor(x => x.Name).NotEmpty();
         RuleFor(x => x.City).NotEmpty();
      }
   }
}
