using Darts.Domain.DomainObjects;
using FluentValidation;

namespace Darts.Domain.Validation
{
   public class LeagueValidator : AbstractValidator<League>
   {
      public LeagueValidator()
      {
         RuleFor(x => x.Name).NotEmpty();
         RuleFor(x => x.ShortCut).NotEmpty();
         RuleFor(x => x.PointsPerWin).GreaterThan(0);
         RuleFor(x => x.UserId).GreaterThan(0);
      }
   }
}
