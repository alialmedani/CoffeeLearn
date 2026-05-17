using CoffeeLearn.Domain.Common;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Domain.Entities;

public class UploadedFile : BaseEntity
{
	public string EntityId { get; set; } = string.Empty;

	public FileEntityType EntityType { get; set; }

	public FilePlacement FilePlacement { get; set; }

	public string OriginalFileName { get; set; } = string.Empty;

	public string StoredFileName { get; set; } = string.Empty;

	public string ContentType { get; set; } = string.Empty;

	public long FileSize { get; set; }

	public string FilePath { get; set; } = string.Empty;

	public string FileUrl { get; set; } = string.Empty;
}