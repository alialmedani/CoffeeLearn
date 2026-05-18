using FluentValidation;

namespace CoffeeLearn.Application.Products.Commands;

public class UpdateProductVariantCommandValidator : AbstractValidator<UpdateProductVariantCommand>
{
	public UpdateProductVariantCommandValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0);

		RuleFor(x => x.Color)
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(x => x.SizeOptionId)
			.NotNull()
			.GreaterThan(0);

		RuleFor(x => x.Quantity)
			.GreaterThanOrEqualTo(0);

		RuleFor(x => x.Sku)
			.MaximumLength(100);

		RuleFor(x => x.ImageUrl)
			.MaximumLength(500);
	}
}



