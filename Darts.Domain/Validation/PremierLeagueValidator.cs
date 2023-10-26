using Darts.Domain.DomainObjects;
using FluentValidation;

namespace Darts.Domain.Validation
{
   public class PremierLeagueValidator : AbstractValidator<PremierLeague>
   {
      public PremierLeagueValidator()
      {
         RuleFor(x => x.Name).NotEmpty();
         RuleFor(x => x.ShortCut).NotEmpty();
         RuleFor(x => x.PointsPerWin).GreaterThan(0);
         RuleFor(x => x.UserId).GreaterThan(0);
      }
   }
}
