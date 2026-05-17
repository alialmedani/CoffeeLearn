using CoffeeLearn.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Common.Extensions;

public static class QueryableExtensions
{
	public static async Task<PagedResult<TDto>> ToPagedResultAsync<TEntity, TDto>(
		this IQueryable<TEntity> query,
		int pageNumber,
		int pageSize,
		Func<TEntity, TDto> mapper,
		CancellationToken cancellationToken = default)
	{
		if (pageNumber <= 0)
			pageNumber = 1;

		if (pageSize <= 0)
			pageSize = 10;

		var totalCount = await query.CountAsync(cancellationToken);

		var items = await query
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		return new PagedResult<TDto>
		{
			Items = items.Select(mapper).ToList(),
			TotalCount = totalCount,
			PageNumber = pageNumber,
			PageSize = pageSize
		};
	}
}