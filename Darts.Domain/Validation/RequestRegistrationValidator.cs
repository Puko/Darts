using Darts.Domain.DomainObjects;
using FluentValidation;

namespace Darts.Domain.Validation
{
    public class RequestRegistrationValidator : AbstractValidator<RequestRegistration>
    {
        public RequestRegistrationValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Message).NotEmpty();
        }
    }
}
