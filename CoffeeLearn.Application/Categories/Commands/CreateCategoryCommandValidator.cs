using FluentValidation;

namespace CoffeeLearn.Application.Categories.Commands;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
	public CreateCategoryCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(200);

		RuleFor(x => x.Description)
			.MaximumLength(1000);

		RuleForEach(x => x.SizeOptions)
			.ChildRules(size =>
			{
				size.RuleFor(x => x.SizeName)
					.NotEmpty()
					.MaximumLength(50);

				size.RuleFor(x => x.SortOrder)
					.GreaterThanOrEqualTo(0);
			});

		RuleFor(x => x.SizeOptions)
			.Must(sizeOptions =>
				sizeOptions
					.Select(x => x.SizeName.Trim().ToLower())
					.Distinct()
					.Count() == sizeOptions.Count)
			.WithMessage("Duplicate size options are not allowed.");
	}
}