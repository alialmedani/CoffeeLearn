using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Queries;

public class GetDeletedProductsQueryHandler : IRequestHandler<GetDeletedProductsQuery, PagedResult<ProductDto>>
{
	private readonly IApplicationDbContext _context;

	public GetDeletedProductsQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<PagedResult<ProductDto>> Handle(GetDeletedProductsQuery request, CancellationToken cancellationToken)
	{
		var query = _context.Products
			.IgnoreQueryFilters()
			.AsNoTracking()
			.Where(x => x.IsDeleted)
			.AsQueryable();

		query = ProductSortingHelper.ApplySorting(
	query,
	request.SortBy,
	request.SortDirection);

		var totalCount = await query.CountAsync(cancellationToken);

		var products = await query
			.Skip((request.PageNumber - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToListAsync(cancellationToken);

		var items = products
			.Select(ProductMapper.ToDto)
			.ToList();

		return new PagedResult<ProductDto>
		{
			Items = items,
			TotalCount = totalCount,
			PageNumber = request.PageNumber,
			PageSize = request.PageSize
		};
	}
}