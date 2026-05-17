using CoffeeLearn.Application.Brands.Common;
using CoffeeLearn.Application.Brands.DTOs;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Extensions;
namespace CoffeeLearn.Application.Brands.Queries;

public class GetDeletedBrandsQueryHandler : IRequestHandler<GetDeletedBrandsQuery, PagedResult<BrandDto>>
{
	private readonly IApplicationDbContext _context;

	public GetDeletedBrandsQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<PagedResult<BrandDto>> Handle(GetDeletedBrandsQuery request, CancellationToken cancellationToken)
	{
		var query = _context.Brands
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

		

		return await query.ToPagedResultAsync(
		request.PageNumber,
		request.PageSize,
		BrandMapper.ToDto,
		cancellationToken);
	}
}