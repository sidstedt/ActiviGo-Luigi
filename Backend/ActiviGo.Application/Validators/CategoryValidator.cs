using ActiviGo.Application.DTOs;
using FluentValidation;

namespace ActiviGo.Application.Validators
{
    public class CategoryValidator : AbstractValidator <CreateCategoryDto>
    {
        public CategoryValidator()//Validation for category creation DTO
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

            RuleFor(c => c.Description)
                .MaximumLength(500).WithMessage("Category description must not exceed 500 characters.");
        }
    }
}
