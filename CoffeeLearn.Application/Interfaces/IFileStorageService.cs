using CoffeeLearn.Application.Common.Models;

namespace CoffeeLearn.Application.Interfaces;

public interface IFileStorageService
{
	Task<FileStorageResult> SaveAsync(
		Stream fileStream,
		string originalFileName,
		string contentType,
		long fileSize,
		string folderName,
		CancellationToken cancellationToken);

	Task DeleteAsync(string filePath, CancellationToken cancellationToken);
}




