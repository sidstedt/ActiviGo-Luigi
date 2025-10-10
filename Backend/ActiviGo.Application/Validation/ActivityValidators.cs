using ActiviGo.Application.DTOs;
using FluentValidation;

namespace ActiviGo.Application.Validation
{
    public sealed class ActivityCreateDtoValidator : AbstractValidator<ActivityCreateDto>
    {
        public ActivityCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name är obligatoriskt.")
                .MaximumLength(100).WithMessage("Name får vara högst 100 tecken.");

            RuleFor(x => x.Description)
                .Must(v => !string.IsNullOrWhiteSpace(v))
                .NotEmpty().WithMessage("Description är obligatoriskt.")
                .MaximumLength(500).WithMessage("Description får vara högst 500 tecken.");

            RuleFor(x => x.Price)
                .InclusiveBetween(0, 500).WithMessage("Price måste vara mellan 0 och 500.");

            RuleFor(x => x.MaxParticipants)
                .InclusiveBetween(1, 50).WithMessage("MaxParticipants måste vara mellan 1 och 50.");

            RuleFor(x => x.DurationMinutes)
                .InclusiveBetween(1, 120).WithMessage("DurationMinutes måste vara mellan 1 och 120.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId måste vara större än 0.");

            RuleFor(x => x.ZoneId)
                .GreaterThan(0).WithMessage("ZoneId måste vara större än 0.");

            RuleFor(x => x.StaffId)
                .Must(id => id == null || id != Guid.Empty)
                .WithMessage("Om StaffId anges får den inte vara tom (Guid.Empty).");
        }
    }
    public sealed class ActivityUpdateDtoValidator : AbstractValidator<ActivityUpdateDto>
    {
        public ActivityUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name är obligatoriskt.")
                .MaximumLength(100).WithMessage("Name får vara högst 100 tecken.");

            RuleFor(x => x.Description)
                .Must(v => !string.IsNullOrWhiteSpace(v))
                .NotEmpty().WithMessage("Description är obligatoriskt.")
                .MaximumLength(500).WithMessage("Description får vara högst 500 tecken.");

            RuleFor(x => x.Price)
                .InclusiveBetween(0, 500).WithMessage("Price måste vara mellan 0 och 500.");

            RuleFor(x => x.MaxParticipants)
                .InclusiveBetween(1, 50).WithMessage("MaxParticipants måste vara mellan 1 och 50.");

            RuleFor(x => x.DurationMinutes)
                .InclusiveBetween(1, 120).WithMessage("DurationMinutes måste vara mellan 1 och 120.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId måste vara större än 0.");

            RuleFor(x => x.ZoneId)
                .GreaterThan(0).WithMessage("ZoneId måste vara större än 0.");

            RuleFor(x => x.StaffId)
                .Must(id => id == null || id != Guid.Empty)
                .WithMessage("Om StaffId anges får den inte vara tom (Guid.Empty).");
        }
    }
}
