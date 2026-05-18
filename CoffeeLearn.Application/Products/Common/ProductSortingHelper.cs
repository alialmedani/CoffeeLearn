using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Products.Common;

public static class ProductSortingHelper
{
	public static IQueryable<CoffeeLearn.Domain.Entities.Product> ApplySorting(
	IQueryable<CoffeeLearn.Domain.Entities.Product> query,
		string? sortBy,
		string? sortDirection)
	{
		var normalizedSortBy = sortBy?.Trim().ToLower();
		var normalizedSortDirection = sortDirection?.Trim().ToLower() ?? "asc";

		return (normalizedSortBy, normalizedSortDirection) switch
		{
			("name", "desc") => query.OrderByDescending(x => x.Name),
			("name", _) => query.OrderBy(x => x.Name),

			("price", "desc") => query.OrderByDescending(x => x.Price),
			("price", _) => query.OrderBy(x => x.Price),

			("quantity", "desc") => query.OrderByDescending(x => x.Quantity),
			("quantity", _) => query.OrderBy(x => x.Quantity),

			("isactive", "desc") => query.OrderByDescending(x => x.IsActive),
			("isactive", _) => query.OrderBy(x => x.IsActive),

			("createdat", "desc") => query.OrderByDescending(x => x.CreatedAt),
			("createdat", _) => query.OrderBy(x => x.CreatedAt),

			("updatedat", "desc") => query.OrderByDescending(x => x.UpdatedAt),
			("updatedat", _) => query.OrderBy(x => x.UpdatedAt),

			("deletedat", "asc") => query.OrderBy(x => x.DeletedAt),
			("deletedat", "desc") => query.OrderByDescending(x => x.DeletedAt),

			("id", "desc") => query.OrderByDescending(x => x.Id),
			_ => query.OrderBy(x => x.Id)
		};
	}
}




