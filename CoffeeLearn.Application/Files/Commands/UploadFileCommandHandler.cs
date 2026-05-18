using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Exceptions;
using CoffeeLearn.Application.Files.Common;
using CoffeeLearn.Application.Files.DTOs;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Domain.Entities;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Files.Commands;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, UploadedFileDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IFileStorageService _fileStorageService;
	private readonly IDateTimeProvider _dateTimeProvider;

	public UploadFileCommandHandler(
		IApplicationDbContext context,
		IFileStorageService fileStorageService,
		IDateTimeProvider dateTimeProvider)
	{
		_context = context;
		_fileStorageService = fileStorageService;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<UploadedFileDto> Handle(UploadFileCommand request, CancellationToken cancellationToken)
	{
		Product? product = null;

		if (request.EntityType == FileEntityType.Product)
		{
			if (!int.TryParse(request.EntityId, out var productId))
				throw new BusinessRuleException("Product EntityId must be a valid integer.");

			product = await _context.Products
				.FirstOrDefaultAsync(x => x.Id == productId, cancellationToken);

			if (product is null)
				throw new NotFoundException("Product does not exist.");
		}

		if (request.EntityType == FileEntityType.Order)
		{
			if (!int.TryParse(request.EntityId, out var orderId))
				throw new BusinessRuleException("Order EntityId must be a valid integer.");

			var orderExists = await _context.Orders
				.AnyAsync(x => x.Id == orderId, cancellationToken);

			if (!orderExists)
				throw new NotFoundException("Order does not exist.");
		}

		if (request.EntityType == FileEntityType.User)
		{
			throw new BusinessRuleException("User file upload is not supported yet.");
		}

		if (request.ReplaceExisting)
		{
			var existingFiles = await _context.UploadedFiles
				.Where(x =>
					x.EntityId == request.EntityId &&
					x.EntityType == request.EntityType &&
					x.FilePlacement == request.FilePlacement)
				.ToListAsync(cancellationToken);

			foreach (var existingFile in existingFiles)
			{
				await _fileStorageService.DeleteAsync(existingFile.FilePath, cancellationToken);
				existingFile.MarkAsDeleted(_dateTimeProvider.UtcNow);
			}
		}

		var folderName = $"{request.EntityType.ToString().ToLower()}/{request.FilePlacement.ToString().ToLower()}";

		var storageResult = await _fileStorageService.SaveAsync(
			request.FileStream,
			request.OriginalFileName,
			request.ContentType,
			request.FileSize,
			folderName,
			cancellationToken);

		var uploadedFile = new UploadedFile
		{
			EntityId = request.EntityId,
			EntityType = request.EntityType,
			FilePlacement = request.FilePlacement,
			OriginalFileName = storageResult.OriginalFileName,
			StoredFileName = storageResult.StoredFileName,
			ContentType = storageResult.ContentType,
			FileSize = storageResult.FileSize,
			FilePath = storageResult.FilePath,
			FileUrl = storageResult.FileUrl
		};

		_context.UploadedFiles.Add(uploadedFile);

		if (request.EntityType == FileEntityType.Product &&
			request.FilePlacement == FilePlacement.MainImage &&
			product is not null)
		{
			product.ImageUrl = storageResult.FileUrl;
		}

		await _context.SaveChangesAsync(cancellationToken);

		return UploadedFileMapper.ToDto(uploadedFile);
	}
}




