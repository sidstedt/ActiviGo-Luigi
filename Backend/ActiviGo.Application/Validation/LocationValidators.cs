using ActiviGo.Application.DTOs;
using FluentValidation;

namespace ActiviGo.Application.Validation
{
    public sealed class LocationCreateDtoValidator : AbstractValidator<LocationCreateDto>
    {
        public LocationCreateDtoValidator()//Validation for location creation DTO
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Namn är obligatoriskt.")
                .MaximumLength(100).WithMessage("Namn får inte vara längre än 100 tecken.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adress är obligatorisk.")
                .MaximumLength(200).WithMessage("Adress får inte vara längre än 200 tecken.");

            // Latitude/Longitude är valfria men om de skickas in ska de vara inom rimliga intervall
            When(x => x.Latitude.HasValue, () =>
            {
                RuleFor(x => x.Latitude!.Value)
                    .InclusiveBetween(-90, 90).WithMessage("Latitude måste vara mellan -90 och 90.");
            });

            When(x => x.Longitude.HasValue, () =>
            {
                RuleFor(x => x.Longitude!.Value)
                    .InclusiveBetween(-180, 180).WithMessage("Longitude måste vara mellan -180 och 180.");
            });
        }
    }
    public sealed class LocationUpdateDtoValidator : AbstractValidator<LocationUpdateDto>
    {
        public LocationUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Namn är obligatoriskt.")
                .MaximumLength(100).WithMessage("Namn får inte vara längre än 100 tecken.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adress är obligatorisk.")
                .MaximumLength(200).WithMessage("Adress får inte vara längre än 200 tecken.");

            // Latitude/Longitude är valfria men om de skickas in ska de vara inom rimliga intervall
            When(x => x.Latitude.HasValue, () =>
            {
                RuleFor(x => x.Latitude!.Value)
                    .InclusiveBetween(-90, 90).WithMessage("Latitude måste vara mellan -90 och 90.");
            });

            When(x => x.Longitude.HasValue, () =>
            {
                RuleFor(x => x.Longitude!.Value)
                    .InclusiveBetween(-180, 180).WithMessage("Longitude måste vara mellan -180 och 180.");
            });
        }
    }
}
