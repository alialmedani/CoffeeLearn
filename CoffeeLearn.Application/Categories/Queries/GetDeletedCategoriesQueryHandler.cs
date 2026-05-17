using CoffeeLearn.Application.Categories.Common;
using CoffeeLearn.Application.Categories.DTOs;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Categories.Queries;

public class GetDeletedCategoriesQueryHandler : IRequestHandler<GetDeletedCategoriesQuery, PagedResult<CategoryDto>>
{
	private readonly IApplicationDbContext _context;

	public GetDeletedCategoriesQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<PagedResult<CategoryDto>> Handle(GetDeletedCategoriesQuery request, CancellationToken cancellationToken)
	{
		var query = _context.Categories
			.IgnoreQueryFilters()
			.AsNoTracking()
			.Where(x => x.IsDeleted)
			.AsQueryable();

		query = request.SortBy?.ToLower() switch
		{
			"name" => request.SortDirection?.ToLower() == "desc"
				? query.OrderByDescending(x => x.Name)
				: query.OrderBy(x => x.Name),

			"createdat" => request.SortDirection?.ToLower() == "desc"
				? query.OrderByDescending(x => x.CreatedAt)
				: query.OrderBy(x => x.CreatedAt),

			"updatedat" => request.SortDirection?.ToLower() == "desc"
				? query.OrderByDescending(x => x.UpdatedAt)
				: query.OrderBy(x => x.UpdatedAt),

			_ => query.OrderByDescending(x => x.Id)
		};

		var totalCount = await query.CountAsync(cancellationToken);

		var categories = await query
			.Skip((request.PageNumber - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToListAsync(cancellationToken);

		var items = categories
			.Select(CategoryMapper.ToDto)
			.ToList();

		return new PagedResult<CategoryDto>
		{
			Items = items,
			TotalCount = totalCount,
			PageNumber = request.PageNumber,
			PageSize = request.PageSize
		};
	}
}