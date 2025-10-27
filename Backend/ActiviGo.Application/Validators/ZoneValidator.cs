using ActiviGo.Application.DTOs;
using ActiviGo.Domain.Models;
using FluentValidation;

namespace ActiviGo.Application.Validators
{
    public class ZoneValidator : AbstractValidator<ZoneDto>
    {
        public ZoneValidator()//Validation for zone DTO
        {
            RuleFor(z => z.Name)
                .NotEmpty().WithMessage("Zone name is required.")
                .MaximumLength(100).WithMessage("Zone name must not exceed 100 characters.");

            RuleFor(z => z.IsOutdoor)
                .NotNull().WithMessage("IsOutdoor field is required.");

            RuleFor(z => z.LocationId)
                .GreaterThan(0).WithMessage("LocationId must be a positive integer.");
        }
    }
}
