using Darts.Domain.DomainObjects;
using FluentValidation;

namespace Darts.Domain.Validation
{
   public class UserValidator : AbstractValidator<User>
   {
      public UserValidator()
      {
         RuleFor(x => x.FirstName).NotEmpty();
         RuleFor(x => x.LastName).NotEmpty();
         RuleFor(x => x.Password).NotEmpty();
         RuleFor(x => x.Email).NotEmpty().EmailAddress();
      }

   }
}
