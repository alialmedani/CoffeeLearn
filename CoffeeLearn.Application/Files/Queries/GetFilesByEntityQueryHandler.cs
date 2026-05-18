using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Files.Common;
using CoffeeLearn.Application.Files.DTOs;
using CoffeeLearn.Application.Interfaces;

namespace CoffeeLearn.Application.Files.Queries;

public class GetFilesByEntityQueryHandler : IRequestHandler<GetFilesByEntityQuery, List<UploadedFileDto>>
{
	private readonly IApplicationDbContext _context;

	public GetFilesByEntityQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<UploadedFileDto>> Handle(GetFilesByEntityQuery request, CancellationToken cancellationToken)
	{
		var query = _context.UploadedFiles
			.AsNoTracking()
			.Where(x =>
				x.EntityId == request.EntityId &&
				x.EntityType == request.EntityType)
			.AsQueryable();

		if (request.FilePlacement.HasValue)
		{
			query = query.Where(x => x.FilePlacement == request.FilePlacement.Value);
		}

		var files = await query
			.OrderByDescending(x => x.CreatedAt)
			.ToListAsync(cancellationToken);

		return files
			.Select(UploadedFileMapper.ToDto)
			.ToList();
	}
}




