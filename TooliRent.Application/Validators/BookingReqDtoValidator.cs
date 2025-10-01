using FluentValidation;
using TooliRent.Application.Dto.Booking;

namespace TooliRent.Application.Validators
{
    public class BookingReqDtoValidator : AbstractValidator<BookingReqDto>
    {
        public BookingReqDtoValidator() 
        {
            RuleFor(b => b.ToolId)
                .NotEmpty().WithMessage("Måste finnas minst ett verktyg i bokningen.")
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
        }
    }
}
