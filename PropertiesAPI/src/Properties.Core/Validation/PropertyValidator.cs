using FluentValidation;
using FluentValidation.Results;
using Properties.Core.Entities;
using Properties.Core.Interfaces;

namespace Properties.Core.Validation
{
    public class PropertyValidator : AbstractValidator<Property>, IEntityValidator<Property>
    {
        public PropertyValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("The name is required")
                .MinimumLength(10).WithMessage("The name must be at least 10 characters")
                .MaximumLength(100).WithMessage("The name must be at most 100 characters");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("The price must be greater than 0");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("The description is required")
                .MinimumLength(30).WithMessage("The description must be at least 30 characters")
                .MaximumLength(250).WithMessage("The description must be at most 250 characters");
        }

        public async Task<IEnumerable<string>> ValidatePropertyAsync(Property entity)
        {
            ValidationResult result = await ValidateAsync(entity);
            return result.Errors.Select(e => e.ErrorMessage);
        }
    }
}