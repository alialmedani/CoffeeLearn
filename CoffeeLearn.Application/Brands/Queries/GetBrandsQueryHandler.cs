using CoffeeLearn.Application.Brands.Common;
using CoffeeLearn.Application.Brands.DTOs;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Extensions;
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
			var search = request.Search.NormalizeText();
			query = query.Where(x => x.Name.ToLower().Contains(search));
		}

		if (request.IsActive.HasValue)
		{
			query = query.Where(x => x.IsActive == request.IsActive.Value);
		}

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





