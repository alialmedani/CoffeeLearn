using CoffeeLearn.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Common.Extensions;

public static class QueryableExtensions
{
	private const int DefaultMaxResultCount = 20;
	private const int MaxAllowedResultCount = 100;

	public static async Task<PagedResult<TDto>> ToPagedResultAsync<TEntity, TDto>(
		this IQueryable<TEntity> query,
		int skipCount,
		int? maxResultCount,
		Func<TEntity, TDto> mapper,
		CancellationToken cancellationToken = default)
	{
		if (skipCount < 0)
			skipCount = 0;

		var take = maxResultCount ?? DefaultMaxResultCount;

		if (take <= 0)
			take = DefaultMaxResultCount;

		if (take > MaxAllowedResultCount)
			take = MaxAllowedResultCount;

		var totalCount = await query.CountAsync(cancellationToken);

		var items = await query
			.Skip(skipCount)
			.Take(take)
			.ToListAsync(cancellationToken);

		return new PagedResult<TDto>
		{
			Items = items.Select(mapper).ToList(),
			TotalCount = totalCount
		};
	}
}


