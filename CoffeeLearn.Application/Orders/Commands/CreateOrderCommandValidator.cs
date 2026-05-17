using FluentValidation;

namespace CoffeeLearn.Application.Orders.Commands;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
	public CreateOrderCommandValidator()
	{
		RuleFor(x => x.UserId)
			.NotEmpty();

		RuleFor(x => x.FloorId)
			.NotEmpty();

		RuleFor(x => x.Items)
			.NotNull()
			.Must(x => x.Count > 0)
			.WithMessage("Order must contain at least one item.");

		RuleForEach(x => x.Items).ChildRules(item =>
		{
			item.RuleFor(x => x.ProductId)
				.GreaterThan(0);

			item.RuleFor(x => x.ProductVariantId)
				.NotNull()
				.WithMessage("ProductVariantId is required for clothing store orders.")
				.GreaterThan(0);

			item.RuleFor(x => x.Quantity)
				.GreaterThan(0);
		});
	}
}