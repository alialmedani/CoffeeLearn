using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Products.Queries;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<ProductDto>>
{
	private readonly IApplicationDbContext _context;

	public GetProductsQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<PagedResult<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
	{
		var query = _context.Products
.Include(x => x.Category)
.Include(x => x.Brand).AsNoTracking()
			.AsQueryable();

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var search = request.Search.Trim().ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(search));
		}

		if (request.CategoryId.HasValue)
		{
			query = query.Where(x => x.CategoryId == request.CategoryId.Value);
		}
		if (request.BrandId.HasValue)
		{
			query = query.Where(x => x.BrandId == request.BrandId.Value);
		}

		if (request.MinPrice.HasValue)
		{
			query = query.Where(x => x.Price >= request.MinPrice.Value);
		}

		if (request.MaxPrice.HasValue)
		{
			query = query.Where(x => x.Price <= request.MaxPrice.Value);
		}

		if (request.IsActive.HasValue)
		{
			query = query.Where(x => x.IsActive == request.IsActive.Value);
		}

		if (request.AvailabilityStatus.HasValue)
		{
			query = request.AvailabilityStatus.Value switch
			{
				ProductAvailabilityStatus.Active =>
					query.Where(x => x.IsActive && x.Quantity > 0),

				ProductAvailabilityStatus.Inactive =>
					query.Where(x => !x.IsActive),

				ProductAvailabilityStatus.OutOfStock =>
					query.Where(x => x.IsActive && x.Quantity <= 0),

				_ => query
			};
		}

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