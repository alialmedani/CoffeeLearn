using CoffeeLearn.Application.Categories.Common;
using CoffeeLearn.Application.Categories.DTOs;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Extensions;
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
		query = CategorySortingHelper.ApplySorting(
			query,
			request.SortBy,
			request.SortDirection);



		return await query.ToPagedResultAsync(
	request.PageNumber,
	request.PageSize,
	CategoryMapper.ToDto,
	cancellationToken);
	}
}