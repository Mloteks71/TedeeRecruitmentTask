using FluentValidation;

namespace Tedee.Models.Validators
{
    public class RegisteredEmailValidator : AbstractValidator<BaseRegisteredEmail>
    {
        public RegisteredEmailValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
