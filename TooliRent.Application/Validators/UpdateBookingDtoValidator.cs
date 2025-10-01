using FluentValidation;
using TooliRent.Application.Dto.Booking;

namespace TooliRent.Application.Validators
{
    public class UpdateBookingDtoValidator : AbstractValidator<UpdateBookingDto>
    {
        public UpdateBookingDtoValidator()
        {
            RuleFor(b => b.ToolId)
                .NotEmpty().WithMessage("ToolId måste vara större än 0.")
                .Must(ids => ids.All(id => id > 0))
                .WithMessage("Alla ToolIds måste vara större än 0.");

            RuleFor(b => b.LoanDate)
                .NotEmpty()
                .WithMessage("Startdatum får inte vara tomt.")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Startdatum måste vara idag eller senare.");

            RuleFor(b => b.ReturnDate)
                .NotEmpty()
                .WithMessage("Slutdatum får inte vara tomt.")
                .GreaterThanOrEqualTo(b => b.LoanDate)
                .WithMessage("Slutdatum måste vara efter utlåningsdatum.");

            RuleFor(b => b.Status)
                .IsInEnum().WithMessage("Status är ogiltig.");




        }
    }
}
