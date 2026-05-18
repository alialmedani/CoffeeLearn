using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Orders.Common;

public static class OrderSortingHelper
{
	public static IQueryable<Order> ApplySorting(
		IQueryable<Order> query,
		string? sortBy,
		string? sortDirection,
		string defaultSortBy = "createdat")
	{
		var normalizedSortBy = sortBy?.Trim().ToLower() ?? defaultSortBy;
		var normalizedSortDirection = sortDirection?.Trim().ToLower() ?? "desc";

		return (normalizedSortBy, normalizedSortDirection) switch
		{
			("id", "asc") => query.OrderBy(x => x.Id),
			("id", _) => query.OrderByDescending(x => x.Id),

			("createdat", "asc") => query.OrderBy(x => x.CreatedAt),
			("createdat", _) => query.OrderByDescending(x => x.CreatedAt),

			("updatedat", "asc") => query.OrderBy(x => x.UpdatedAt),
			("updatedat", _) => query.OrderByDescending(x => x.UpdatedAt),

			("acceptedat", "asc") => query.OrderBy(x => x.AcceptedAt),
			("acceptedat", _) => query.OrderByDescending(x => x.AcceptedAt),

			("completedat", "asc") => query.OrderBy(x => x.CompletedAt),
			("completedat", _) => query.OrderByDescending(x => x.CompletedAt),

			("cancelledat", "asc") => query.OrderBy(x => x.CancelledAt),
			("cancelledat", _) => query.OrderByDescending(x => x.CancelledAt),

			("status", "asc") => query.OrderBy(x => x.Status),
			("status", _) => query.OrderByDescending(x => x.Status),

			_ => query.OrderByDescending(x => x.CreatedAt)
		};
	}
}




