using FluentValidation;

namespace CoffeeLearn.Application.Products.Commands;

public class DecreaseProductVariantStockCommandValidator
	: AbstractValidator<DecreaseProductVariantStockCommand>
{
	public DecreaseProductVariantStockCommandValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0);

		RuleFor(x => x.Quantity)
			.GreaterThan(0);
	}
}