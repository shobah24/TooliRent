using FluentValidation;
using TooliRent.Application.Dto.Category;

namespace TooliRent.Application.Validators
{
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator() 
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Kategorinamn får inte vara tomt.")
                .MaximumLength(100).WithMessage("Kategorinamn får inte överstiga 100 tecken.");
        }
    }
}
