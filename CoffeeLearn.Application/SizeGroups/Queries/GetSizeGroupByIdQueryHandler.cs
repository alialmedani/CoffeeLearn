using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.SizeGroups.Common;
using CoffeeLearn.Application.SizeGroups.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.SizeGroups.Queries;

public class GetSizeGroupByIdQueryHandler : IRequestHandler<GetSizeGroupByIdQuery, SizeGroupDto?>
{
	private readonly IApplicationDbContext _context;

	public GetSizeGroupByIdQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<SizeGroupDto?> Handle(GetSizeGroupByIdQuery request, CancellationToken cancellationToken)
	{
		var sizeGroup = await _context.SizeGroups
			.AsNoTracking()
			.Include(x => x.Category)
			.Include(x => x.SizeOptions)
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (sizeGroup is null)
			return null;

		return SizeGroupMapper.ToDto(sizeGroup);
	}
}


