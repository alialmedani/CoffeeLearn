using FluentValidation;

namespace CoffeeLearn.Application.Brands.Commands;

public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
	public UpdateBrandCommandValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0);

		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(200);

		RuleFor(x => x.Description)
			.MaximumLength(1000);
	}
}





