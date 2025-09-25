using FluentValidation;
using TooliRent.Application.Dto.Tool;

namespace TooliRent.Application.Validators
{
    public class CreateToolDtoValidator : AbstractValidator<CreateToolDto>
    {
        public CreateToolDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Namn är obligatoriskt")
                .MaximumLength(100).WithMessage("Namnet kan vara max 100 tecken");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Beskrivning är obligatorisk")
                .MaximumLength(500).WithMessage("Beskrivningen kan vara max 500 tecken");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId måste vara större än 0");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status måste vara en giltig enum");
        }

    }
}
