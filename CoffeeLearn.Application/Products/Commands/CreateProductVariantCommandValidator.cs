using FluentValidation;

namespace CoffeeLearn.Application.Products.Commands;

public class CreateProductVariantCommandValidator : AbstractValidator<CreateProductVariantCommand>
{
	public CreateProductVariantCommandValidator()
	{
		RuleFor(x => x.ProductId)
			.GreaterThan(0);

		RuleFor(x => x.Color)
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(x => x.Size)
			.NotEmpty()
			.MaximumLength(50);

		RuleFor(x => x.Quantity)
			.GreaterThanOrEqualTo(0);

		RuleFor(x => x.Sku)
			.MaximumLength(100);
	}
}