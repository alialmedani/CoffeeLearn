using CoffeeLearn.Application.Categories.Common;
using CoffeeLearn.Application.Categories.DTOs;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Extensions;

namespace CoffeeLearn.Application.Categories.Queries;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PagedResult<CategoryDto>>
{
	private readonly IApplicationDbContext _context;

	public GetCategoriesQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<PagedResult<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
	{
		var query = _context.Categories
			.AsNoTracking()
 			.AsQueryable();

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var search = request.Search.NormalizeText();
			query = query.Where(x => x.Name.ToLower().Contains(search));
		}

		if (request.IsActive.HasValue)
		{
			query = query.Where(x => x.IsActive == request.IsActive.Value);
		}

		query = CategorySortingHelper.ApplySorting(
			query,
			request.SortBy,
			request.SortDirection);

		return await query.ToPagedResultAsync(
			request.SkipCount,
request.MaxResultCount,
			CategoryMapper.ToDto,
			cancellationToken);
	}
}





