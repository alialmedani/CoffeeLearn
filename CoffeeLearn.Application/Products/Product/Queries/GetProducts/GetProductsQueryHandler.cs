using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;
using CoffeeLearn.Domain.Enums;
using CoffeeLearn.Application.Common.Extensions;

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
			.IncludeProductDetails()
			.AsNoTracking()
			.AsQueryable();

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var search = request.Search.NormalizeText();
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
					query.Where(x =>
						x.IsActive &&
						x.Variants.Any(v => v.IsActive && !v.IsDeleted && v.Quantity > 0)),

				ProductAvailabilityStatus.Inactive =>
					query.Where(x => !x.IsActive),

				ProductAvailabilityStatus.OutOfStock =>
					query.Where(x =>
						x.IsActive &&
						!x.Variants.Any(v => v.IsActive && !v.IsDeleted && v.Quantity > 0)),

				_ => query
			};
		}

		query = ProductSortingHelper.ApplySorting(
			query,
			request.SortBy,
			request.SortDirection);

		return await query.ToPagedResultAsync(
			request.SkipCount,
			request.MaxResultCount,
			ProductMapper.ToDto,
			cancellationToken);
	}
}

