using FluentValidation;

namespace CoffeeLearn.Application.Products.Commands;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
	public UpdateProductCommandValidator()
	{
		RuleFor(x => x.Id)
			.GreaterThan(0);

		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(200);

		RuleFor(x => x.Description)
			.MaximumLength(1000);

		RuleFor(x => x.ImageUrl)
			.MaximumLength(500)
			.Must(BeAValidUrl)
			.When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
			.WithMessage("ImageUrl must be a valid URL.");

		RuleFor(x => x.Quantity)
			.GreaterThanOrEqualTo(0);

		RuleFor(x => x.Price)
			.GreaterThanOrEqualTo(0);
	}

	private static bool BeAValidUrl(string? url)
	{
		return Uri.TryCreate(url, UriKind.Absolute, out var result)
			&& (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
	}
}






