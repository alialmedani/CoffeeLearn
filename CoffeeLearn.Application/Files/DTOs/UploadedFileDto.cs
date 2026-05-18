using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Files.DTOs;

public class UploadedFileDto
{
	public int Id { get; set; }

	public string EntityId { get; set; } = string.Empty;

	public FileEntityType EntityType { get; set; }

	public FilePlacement FilePlacement { get; set; }

	public string OriginalFileName { get; set; } = string.Empty;

	public string StoredFileName { get; set; } = string.Empty;

	public string ContentType { get; set; } = string.Empty;

	public long FileSize { get; set; }

	public string FileUrl { get; set; } = string.Empty;

	public DateTime CreatedAt { get; set; }
}





