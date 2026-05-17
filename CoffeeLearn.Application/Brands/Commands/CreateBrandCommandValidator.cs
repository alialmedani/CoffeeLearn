using FluentValidation;

namespace CoffeeLearn.Application.Brands.Commands;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
	public CreateBrandCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(200);

		RuleFor(x => x.Description)
			.MaximumLength(1000);
	}
}