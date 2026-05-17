using MediatR;
using CoffeeLearn.Application.Files.DTOs;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Files.Commands;

public class UploadFileCommand : IRequest<UploadedFileDto>
{
	public string EntityId { get; set; } = string.Empty;

	public FileEntityType EntityType { get; set; }

	public FilePlacement FilePlacement { get; set; }

	public bool ReplaceExisting { get; set; }

	public Stream FileStream { get; set; } = Stream.Null;

	public string OriginalFileName { get; set; } = string.Empty;

	public string ContentType { get; set; } = string.Empty;

	public long FileSize { get; set; }
}