using Darts.Domain.DomainObjects;
using FluentValidation;

namespace Darts.Domain.Validation
{
   public class MatchValidator : AbstractValidator<Match>
   {
      public MatchValidator()
      {
         RuleFor(x => x.LeagueId).GreaterThan(0);
      }
   }
}
