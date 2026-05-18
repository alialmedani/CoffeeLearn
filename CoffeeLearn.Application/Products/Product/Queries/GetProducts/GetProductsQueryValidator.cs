using FluentValidation;

namespace CoffeeLearn.Application.Products.Queries;

public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
	public GetProductsQueryValidator()
	{
		RuleFor(x => x.SkipCount)
	.GreaterThanOrEqualTo(0);

		RuleFor(x => x.MaxResultCount)
			.GreaterThan(0)
			.LessThanOrEqualTo(100)
			.When(x => x.MaxResultCount.HasValue);

		RuleFor(x => x.MinPrice)
			.GreaterThanOrEqualTo(0)
			.When(x => x.MinPrice.HasValue);

		RuleFor(x => x.MaxPrice)
			.GreaterThanOrEqualTo(0)
			.When(x => x.MaxPrice.HasValue);

		RuleFor(x => x)
			.Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
			.WithMessage("MinPrice must be less than or equal to MaxPrice.");

		RuleFor(x => x.SortBy)
			.Must(x => string.IsNullOrWhiteSpace(x) ||
					   x.Equals("name", StringComparison.OrdinalIgnoreCase) ||
					   x.Equals("price", StringComparison.OrdinalIgnoreCase) ||
					   x.Equals("quantity", StringComparison.OrdinalIgnoreCase) ||
					   x.Equals("id", StringComparison.OrdinalIgnoreCase))
			.WithMessage("SortBy must be one of: id, name, price, quantity.");

		RuleFor(x => x.SortDirection)
			.Must(x => string.IsNullOrWhiteSpace(x) ||
					   x.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
					   x.Equals("desc", StringComparison.OrdinalIgnoreCase))
			.WithMessage("SortDirection must be either 'asc' or 'desc'.");
	}
}




