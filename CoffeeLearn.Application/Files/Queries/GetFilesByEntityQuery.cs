using MediatR;
using CoffeeLearn.Application.Files.DTOs;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Files.Queries;

public class GetFilesByEntityQuery : IRequest<List<UploadedFileDto>>
{
	public string EntityId { get; set; } = string.Empty;

	public FileEntityType EntityType { get; set; }

	public FilePlacement? FilePlacement { get; set; }
}