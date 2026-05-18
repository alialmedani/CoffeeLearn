using FluentValidation;

namespace CoffeeLearn.Application.Products.Commands.ProductWithVariants.CreateProductWithVariants;

public class CreateProductWithVariantsCommandValidator : AbstractValidator<CreateProductWithVariantsCommand>
{
	public CreateProductWithVariantsCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(200);

		RuleFor(x => x.Price)
			.GreaterThanOrEqualTo(0);

		RuleFor(x => x.Description)
			.MaximumLength(1000);

		RuleFor(x => x.ImageUrl)
			.MaximumLength(500)
			.Must(BeAValidUrl)
			.When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
			.WithMessage("ImageUrl must be a valid URL.");

		RuleFor(x => x.Variants)
			.NotNull()
			.Must(x => x.Count > 0)
			.WithMessage("At least one variant is required.");

		RuleForEach(x => x.Variants).ChildRules(variant =>
		{
			variant.RuleFor(x => x.Color)
				.NotEmpty()
				.MaximumLength(100);

			variant.RuleFor(x => x.Size)
				.NotEmpty()
				.MaximumLength(50);

			variant.RuleFor(x => x.Quantity)
				.GreaterThanOrEqualTo(0);

			variant.RuleFor(x => x.Sku)
				.MaximumLength(100);

			variant.RuleFor(x => x.ImageUrl)
				.MaximumLength(500)
				.Must(BeAValidUrl)
				.When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
				.WithMessage("Variant ImageUrl must be a valid URL.");
		});
	}

	private static bool BeAValidUrl(string? url)
	{
		return Uri.TryCreate(url, UriKind.Absolute, out var result)
			&& (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
	}
}