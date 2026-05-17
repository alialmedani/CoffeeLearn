using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Files.Common;
using CoffeeLearn.Application.Files.DTOs;
using CoffeeLearn.Application.Interfaces;

namespace CoffeeLearn.Application.Files.Queries;

public class GetFileByIdQueryHandler : IRequestHandler<GetFileByIdQuery, UploadedFileDto?>
{
	private readonly IApplicationDbContext _context;

	public GetFileByIdQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<UploadedFileDto?> Handle(GetFileByIdQuery request, CancellationToken cancellationToken)
	{
		var file = await _context.UploadedFiles
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (file is null)
			return null;

		return UploadedFileMapper.ToDto(file);
	}
}