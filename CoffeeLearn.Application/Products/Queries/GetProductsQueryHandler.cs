using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.DTOs;

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
			.AsNoTracking()
			.AsQueryable();

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var search = request.Search.Trim().ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(search));
		}

		if (request.MinPrice.HasValue)
		{
			query = query.Where(x => x.Price >= request.MinPrice.Value);
		}

		if (request.MaxPrice.HasValue)
		{
			query = query.Where(x => x.Price <= request.MaxPrice.Value);
		}

		var sortBy = request.SortBy?.Trim().ToLower();
		var sortDirection = request.SortDirection?.Trim().ToLower() ?? "asc";

		query = (sortBy, sortDirection) switch
		{
			("name", "desc") => query.OrderByDescending(x => x.Name),
			("name", _) => query.OrderBy(x => x.Name),

			("price", "desc") => query.OrderByDescending(x => x.Price),
			("price", _) => query.OrderBy(x => x.Price),

			("quantity", "desc") => query.OrderByDescending(x => x.Quantity),
			("quantity", _) => query.OrderBy(x => x.Quantity),

			("id", "desc") => query.OrderByDescending(x => x.Id),
			_ => query.OrderBy(x => x.Id)
		};

		var totalCount = await query.CountAsync(cancellationToken);

		var items = await query
			.Skip((request.PageNumber - 1) * request.PageSize)
			.Take(request.PageSize)
			.Select(x => new ProductDto
			{
				Id = x.Id,
				Name = x.Name,
				Quantity = x.Quantity,
				Price = x.Price
			})
			.ToListAsync(cancellationToken);

		return new PagedResult<ProductDto>
		{
			Items = items,
			TotalCount = totalCount,
			PageNumber = request.PageNumber,
			PageSize = request.PageSize
		};
	}
}