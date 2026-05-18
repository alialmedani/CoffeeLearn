using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Categories.Common;

public static class CategorySortingHelper
{
	public static IQueryable<Category> ApplySorting(
		IQueryable<Category> query,
		string? sortBy,
		string? sortDirection)
	{
		var isDescending = sortDirection?.ToLower() == "desc";

		return sortBy?.ToLower() switch
		{
			"name" => isDescending
				? query.OrderByDescending(x => x.Name)
				: query.OrderBy(x => x.Name),

			"createdat" => isDescending
				? query.OrderByDescending(x => x.CreatedAt)
				: query.OrderBy(x => x.CreatedAt),

			"updatedat" => isDescending
				? query.OrderByDescending(x => x.UpdatedAt)
				: query.OrderBy(x => x.UpdatedAt),

			"isactive" => isDescending
				? query.OrderByDescending(x => x.IsActive)
				: query.OrderBy(x => x.IsActive),

			_ => query.OrderBy(x => x.Id)
		};
	}
}




