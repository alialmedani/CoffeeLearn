using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Api.Models;

public class UploadFileRequest
{
	public string EntityId { get; set; } = string.Empty;

	public FileEntityType EntityType { get; set; }

	public FilePlacement FilePlacement { get; set; }

	public bool ReplaceExisting { get; set; }

	public IFormFile File { get; set; } = default!;
}