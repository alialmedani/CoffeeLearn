using FluentValidation;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
	public GetOrdersQueryValidator()
	{
		RuleFor(x => x.SkipCount)
	.GreaterThanOrEqualTo(0);

		RuleFor(x => x.MaxResultCount)
			.GreaterThan(0)
			.LessThanOrEqualTo(100)
			.When(x => x.MaxResultCount.HasValue);

		RuleFor(x => x.Status)
			.Must(x => string.IsNullOrWhiteSpace(x) ||
					   x.Equals("Pending", StringComparison.OrdinalIgnoreCase) ||
					   x.Equals("Accepted", StringComparison.OrdinalIgnoreCase) ||
					   x.Equals("Completed", StringComparison.OrdinalIgnoreCase) ||
					   x.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
			.WithMessage("Status must be one of: Pending, Accepted, Completed, Cancelled.");

		RuleFor(x => x.SortBy)
			.Must(x => string.IsNullOrWhiteSpace(x) ||
					   x.Equals("id", StringComparison.OrdinalIgnoreCase) ||
					   x.Equals("createdAt", StringComparison.OrdinalIgnoreCase) ||
					   x.Equals("updatedAt", StringComparison.OrdinalIgnoreCase) ||
					   x.Equals("acceptedAt", StringComparison.OrdinalIgnoreCase) ||
					   x.Equals("completedAt", StringComparison.OrdinalIgnoreCase) ||
					   x.Equals("cancelledAt", StringComparison.OrdinalIgnoreCase) ||
					   x.Equals("status", StringComparison.OrdinalIgnoreCase))
			.WithMessage("SortBy must be one of: id, createdAt, updatedAt, acceptedAt, completedAt, cancelledAt, status.");

		RuleFor(x => x.SortDirection)
			.Must(x => string.IsNullOrWhiteSpace(x) ||
					   x.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
					   x.Equals("desc", StringComparison.OrdinalIgnoreCase))
			.WithMessage("SortDirection must be either 'asc' or 'desc'.");
	}
}





