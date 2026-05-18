using CoffeeLearn.Application.Files.DTOs;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Files.Common;

public static class UploadedFileMapper
{
	public static UploadedFileDto ToDto(UploadedFile file)
	{
		return new UploadedFileDto
		{
			Id = file.Id,
			EntityId = file.EntityId,
			EntityType = file.EntityType,
			FilePlacement = file.FilePlacement,
			OriginalFileName = file.OriginalFileName,
			StoredFileName = file.StoredFileName,
			ContentType = file.ContentType,
			FileSize = file.FileSize,
			FileUrl = file.FileUrl,
			CreatedAt = file.CreatedAt
		};
	}
}




