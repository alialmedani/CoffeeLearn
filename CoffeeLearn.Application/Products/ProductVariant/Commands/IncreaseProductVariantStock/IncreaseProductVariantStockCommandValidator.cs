using FluentValidation;

namespace CoffeeLearn.Application.Products.Commands;

public class IncreaseProductVariantStockCommandValidator
	: AbstractValidator<IncreaseProductVariantStockCommand>
{
	public IncreaseProductVariantStockCommandValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0);

		RuleFor(x => x.Quantity)
			.GreaterThan(0);
	}
}