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
		var finalSortBy = string.IsNullOrWhiteSpace(sortBy)
			? defaultSortBy
			: sortBy.Trim().ToLower();

		var finalSortDirection = string.IsNullOrWhiteSpace(sortDirection)
			? "desc"
			: sortDirection.Trim().ToLower();

		return (finalSortBy, finalSortDirection) switch
		{
			("id", "asc") => query.OrderBy(x => x.Id),
			("id", _) => query.OrderByDescending(x => x.Id),

			("acceptedat", "asc") => query.OrderBy(x => x.AcceptedAt),
			("acceptedat", _) => query.OrderByDescending(x => x.AcceptedAt),

			("completedat", "asc") => query.OrderBy(x => x.CompletedAt),
			("completedat", _) => query.OrderByDescending(x => x.CompletedAt),

			("status", "asc") => query.OrderBy(x => x.Status),
			("status", _) => query.OrderByDescending(x => x.Status),

			("createdat", "asc") => query.OrderBy(x => x.CreatedAt),
			_ => query.OrderByDescending(x => x.CreatedAt)
		};
	}
}