using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Files.Commands;

public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, bool>
{
	private readonly IApplicationDbContext _context;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly IFileStorageService _fileStorageService;

	public DeleteFileCommandHandler(
		IApplicationDbContext context,
		IDateTimeProvider dateTimeProvider,
		IFileStorageService fileStorageService)
	{
		_context = context;
		_dateTimeProvider = dateTimeProvider;
		_fileStorageService = fileStorageService;
	}

	public async Task<bool> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
	{
		var file = await _context.UploadedFiles
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (file is null)
			return false;

		if (file.EntityType == FileEntityType.Product &&
			file.FilePlacement == FilePlacement.MainImage &&
			int.TryParse(file.EntityId, out var productId))
		{
			var product = await _context.Products
				.FirstOrDefaultAsync(x => x.Id == productId, cancellationToken);

			if (product is not null && product.ImageUrl == file.FileUrl)
			{
				var replacementImage = await _context.UploadedFiles
					.Where(x =>
						x.Id != file.Id &&
						x.EntityId == file.EntityId &&
						x.EntityType == FileEntityType.Product &&
						x.FilePlacement == FilePlacement.MainImage)
					.OrderByDescending(x => x.CreatedAt)
					.FirstOrDefaultAsync(cancellationToken);

				product.ImageUrl = replacementImage?.FileUrl;
			}
		}

		await _fileStorageService.DeleteAsync(file.FilePath, cancellationToken);

		file.MarkAsDeleted(_dateTimeProvider.UtcNow);

		await _context.SaveChangesAsync(cancellationToken);

		return true;
	}
}




