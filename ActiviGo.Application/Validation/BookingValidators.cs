using ActiviGo.Application.DTOs;
using FluentValidation;

namespace ActiviGo.Application.Validation
{
    public sealed class BookingDtoValidator : AbstractValidator<BookingDto>
    {
        public BookingDtoValidator()
        {
            RuleFor(b => b.Id).GreaterThan(0);
            RuleFor(b => b.UserId).NotEmpty();
            RuleFor(b => b.ActivityOccurrenceId).GreaterThan(0);
            RuleFor(b => b.ActivityId).GreaterThan(0);
            RuleFor(b => b.ActivityName)
                .Must(v => !string.IsNullOrWhiteSpace(v))
                .WithMessage("ActivityName is required")
                .MaximumLength(100);
            RuleFor(b => b.ActivityDescription)
                .Must(v => !string.IsNullOrWhiteSpace(v))
                .WithMessage("ActivityDescription is required")
                .MaximumLength(500);
            RuleFor(b => b.ActivityPrice)
                .GreaterThanOrEqualTo(0)
                .Must(p => decimal.Round(p, 2) == p)
                .WithMessage("ActivityPrice must have at most 2 decimals.");
            RuleFor(b => b.EndTime).GreaterThan(b => b.StartTime);
            RuleFor(b => b.ZoneName)
                .Must(v => !string.IsNullOrWhiteSpace(v))
                .WithMessage("ZoneName is required")
                .MaximumLength(100);
            RuleFor(b => b.CategoryName)
                .Must(v => !string.IsNullOrWhiteSpace(v))
                .WithMessage("CategoryName is required")
                .MaximumLength(100);
            RuleFor(b => b.Status).IsInEnum();
        }
    }
}
