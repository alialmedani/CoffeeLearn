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

		query = BrandSortingHelper.ApplySorting(
		query,
		request.SortBy,
		request.SortDirection);



		return await query.ToPagedResultAsync(
		request.SkipCount,
request.MaxResultCount,
		BrandMapper.ToDto,
		cancellationToken);
	}
}




