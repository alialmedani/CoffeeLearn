using CoffeeLearn.Application.SizeGroups.DTOs;
using MediatR;

namespace CoffeeLearn.Application.SizeGroups.Commands;

public class CreateSizeGroupCommand : IRequest<SizeGroupDto>
{
	public int CategoryId { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }

	public List<CreateSizeOptionRequest> SizeOptions { get; set; } = new();
}


