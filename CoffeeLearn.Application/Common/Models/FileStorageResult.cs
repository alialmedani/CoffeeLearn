namespace CoffeeLearn.Application.Common.Models;

public class FileStorageResult
{
	public string OriginalFileName { get; set; } = string.Empty;
	public string StoredFileName { get; set; } = string.Empty;
	public string ContentType { get; set; } = string.Empty;
	public long FileSize { get; set; }

	public string FilePath { get; set; } = string.Empty;
	public string FileUrl { get; set; } = string.Empty;
}





