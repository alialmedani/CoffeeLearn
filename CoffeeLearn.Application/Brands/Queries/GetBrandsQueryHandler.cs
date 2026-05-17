using CoffeeLearn.Application.Brands.Common;
using CoffeeLearn.Application.Brands.DTOs;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Brands.Queries;

public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, PagedResult<BrandDto>>
{
	private readonly IApplicationDbContext _context;

	public GetBrandsQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<PagedResult<BrandDto>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
	{
		var query = _context.Brands
			.AsNoTracking()
			.AsQueryable();

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var search = request.Search.Trim().ToLower();
			query = query.Where(x => x.Name.ToLower().Contains(search));
		}

		if (request.IsActive.HasValue)
		{
			query = query.Where(x => x.IsActive == request.IsActive.Value);
		}

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

			"isactive" => request.SortDirection?.ToLower() == "desc"
				? query.OrderByDescending(x => x.IsActive)
				: query.OrderBy(x => x.IsActive),

			_ => query.OrderBy(x => x.Id)
		};

		var totalCount = await query.CountAsync(cancellationToken);

		var brands = await query
			.Skip((request.PageNumber - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToListAsync(cancellationToken);

		var items = brands
			.Select(BrandMapper.ToDto)
			.ToList();

		return new PagedResult<BrandDto>
		{
			Items = items,
			TotalCount = totalCount,
			PageNumber = request.PageNumber,
			PageSize = request.PageSize
		};
	}
}