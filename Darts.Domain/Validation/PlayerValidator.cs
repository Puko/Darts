using Darts.Domain.DomainObjects;
using FluentValidation;

namespace Darts.Domain.Validation
{
   public class PlayerValidator : AbstractValidator<Player>
   {
      public PlayerValidator()
      {
         RuleFor(x => x.FirstName).NotEmpty();
         RuleFor(x => x.LastName).NotEmpty();
         RuleFor(x => x.Identifier).NotEmpty();
      }
   }
}
