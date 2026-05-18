using CoffeeLearn.Application.Common.Extensions;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.SizeGroups.Common;
using CoffeeLearn.Application.SizeGroups.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.SizeGroups.Queries;

public class GetSizeGroupsQueryHandler : IRequestHandler<GetSizeGroupsQuery, PagedResult<SizeGroupDto>>
{
	private readonly IApplicationDbContext _context;

	public GetSizeGroupsQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<PagedResult<SizeGroupDto>> Handle(GetSizeGroupsQuery request, CancellationToken cancellationToken)
	{
		var query = _context.SizeGroups
			.AsNoTracking()
			.Include(x => x.Category)
			.Include(x => x.SizeOptions)
			.AsQueryable();

		if (request.CategoryId.HasValue)
			query = query.Where(x => x.CategoryId == request.CategoryId.Value);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var search = request.Search.NormalizeText();
			query = query.Where(x => x.Name.ToLower().Contains(search));
		}

		if (request.IsActive.HasValue)
			query = query.Where(x => x.IsActive == request.IsActive.Value);

		query = query.OrderBy(x => x.Category.Name).ThenBy(x => x.Name);

		return await query.ToPagedResultAsync(
			request.SkipCount,
			request.MaxResultCount,
			SizeGroupMapper.ToDto,
			cancellationToken);
	}
}


