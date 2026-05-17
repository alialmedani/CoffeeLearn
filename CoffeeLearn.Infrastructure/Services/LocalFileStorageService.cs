using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;

namespace CoffeeLearn.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
	public async Task<FileStorageResult> SaveAsync(
		Stream fileStream,
		string originalFileName,
		string contentType,
		long fileSize,
		string folderName,
		CancellationToken cancellationToken)
	{
		if (fileStream is null)
			throw new ArgumentNullException(nameof(fileStream));

		if (string.IsNullOrWhiteSpace(originalFileName))
			throw new ArgumentException("Original file name is required.", nameof(originalFileName));

		var extension = Path.GetExtension(originalFileName);
		var storedFileName = $"{Guid.NewGuid():N}{extension}";

		var safeFolderName = folderName.Trim().ToLower();
		var relativeFolderPath = Path.Combine("uploads", safeFolderName);

		var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
		var fullFolderPath = Path.Combine(rootPath, relativeFolderPath);

		Directory.CreateDirectory(fullFolderPath);

		var fullFilePath = Path.Combine(fullFolderPath, storedFileName);

		await using var outputStream = new FileStream(fullFilePath, FileMode.CreateNew);
		await fileStream.CopyToAsync(outputStream, cancellationToken);

		var normalizedRelativePath = Path.Combine(relativeFolderPath, storedFileName)
			.Replace("\\", "/");

		return new FileStorageResult
		{
			OriginalFileName = originalFileName,
			StoredFileName = storedFileName,
			ContentType = contentType,
			FileSize = fileSize,
			FilePath = normalizedRelativePath,
			FileUrl = $"/{normalizedRelativePath}"
		};
	}

	public Task DeleteAsync(string filePath, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(filePath))
			return Task.CompletedTask;

		var normalizedFilePath = filePath
			.Replace("/", Path.DirectorySeparatorChar.ToString())
			.TrimStart(Path.DirectorySeparatorChar);

		var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
		var fullPath = Path.GetFullPath(Path.Combine(rootPath, normalizedFilePath));

		var uploadsRoot = Path.GetFullPath(Path.Combine(rootPath, "uploads"));

		if (!fullPath.StartsWith(uploadsRoot, StringComparison.OrdinalIgnoreCase))
			throw new InvalidOperationException("Invalid file path.");

		if (File.Exists(fullPath))
			File.Delete(fullPath);

		return Task.CompletedTask;
	}
}