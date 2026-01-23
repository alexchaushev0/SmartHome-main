using SmartHome.Models.Requests;
using FluentValidation;

namespace SmartHome.Host.Validators
{
    public class AddRoomRequestValidator : AbstractValidator<AddRoomRequest>
    {
        public AddRoomRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(2).WithMessage("Името на стаята трябва да е поне 2 символа.");
            RuleFor(x => x.Temperature).InclusiveBetween(15, 30).WithMessage("Температурата трябва да е между 15 и 30 градуса.");
        }
    }
}