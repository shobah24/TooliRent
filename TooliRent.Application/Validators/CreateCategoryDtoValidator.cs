using FluentValidation;
using TooliRent.Application.Dto.Category;

namespace TooliRent.Application.Validators
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator() 
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Kategorinamn får inte vara tomt.")
                .MaximumLength(100).WithMessage("Kategorinamn får inte överstiga 100 tecken.");

        }
    }
}
