using ActiviGo.Application.DTOs;
using FluentValidation;

namespace ActiviGo.Application.Validation
{
    public sealed class ActivityOccurrenceCreateDtoValidator : AbstractValidator<ActivityOccurrenceCreateDto>
    {
        public ActivityOccurrenceCreateDtoValidator()//Validation for activityOccurrence creation DTO
        {
            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("StartTime är obligatoriskt.")
                .Must(start => start > DateTime.UtcNow.AddMinutes(-5))
                .WithMessage("StartTime måste vara i framtiden eller nära nutid.");

            RuleFor(x => x.DurationMinutes)
                .Cascade(CascadeMode.Stop)
                .Must(v => v == null || (v >= 1 && v <= 120))
                .WithMessage("DurationMinutes måste vara mellan 1 och 120 om det anges.");

            RuleFor(x => x.ActivityId)
                .GreaterThan(0).WithMessage("ActivityId måste vara större än 0.");

            RuleFor(x => x.ZoneId)
                .NotNull().WithMessage("ZoneId är obligatoriskt.")
                .GreaterThan(0).WithMessage("ZoneId måste vara större än 0.");
        }
    }
    public sealed class ActivityOccurrenceUpdateDtoValidator : AbstractValidator<ActivityOccurrenceUpdateDto>
    {
        public ActivityOccurrenceUpdateDtoValidator()//Validation for activityOccurrence update DTO
        {
            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("StartTime är obligatoriskt.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("EndTime är obligatoriskt.")
                .GreaterThan(x => x.StartTime)
                .WithMessage("EndTime måste vara senare än StartTime.");

            RuleFor(x => x.DurationMinutes)
                .InclusiveBetween(1, 120).WithMessage("DurationMinutes måste vara mellan 1 och 120.");

            RuleFor(x => x.ActivityId)
                .GreaterThan(0).WithMessage("ActivityId måste vara större än 0.");

            RuleFor(x => x.ZoneId)
                .GreaterThan(0).WithMessage("ZoneId måste vara större än 0.");

            // Konsistens: EndTime ska motsvara StartTime + DurationMinutes
            RuleFor(x => x)
                .Must(x =>
                {
                    var diffMinutes = (x.EndTime - x.StartTime).TotalMinutes;
                    return Math.Abs(diffMinutes - x.DurationMinutes) < 0.0001 && Math.Abs(diffMinutes % 1) < 0.0001;
                })
                .WithMessage("EndTime ska vara StartTime + DurationMinutes (i hela minuter).");
        }
    }
}
