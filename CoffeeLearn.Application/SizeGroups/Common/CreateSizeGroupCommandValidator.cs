using FluentValidation;

namespace CoffeeLearn.Application.SizeGroups.Commands;

public class CreateSizeGroupCommandValidator : AbstractValidator<CreateSizeGroupCommand>
{
	public CreateSizeGroupCommandValidator()
	{
		RuleFor(x => x.CategoryId)
			.GreaterThan(0);

		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(200);

		RuleFor(x => x.Description)
			.MaximumLength(1000);

		RuleForEach(x => x.SizeOptions)
			.ChildRules(option =>
			{
				option.RuleFor(x => x.Name)
					.NotEmpty()
					.MaximumLength(50);

				option.RuleFor(x => x.SortOrder)
					.GreaterThanOrEqualTo(0);
			});
	}
}


