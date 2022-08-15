using FluentValidation;

namespace Tedee.Models.Validators
{
    public class TripValidator : AbstractValidator<BaseTrip>
    {
        public TripValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Country).IsInEnum();
            RuleFor(x => x.Description).NotEmpty().MaximumLength(5000);
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.SeatsCount).NotEmpty();
        }
    }
}
