using CoffeeLearn.Application.Categories.DTOs;
using MediatR;

namespace CoffeeLearn.Application.Categories.Commands;

public class CreateCategoryCommand : IRequest<CategoryDto>
{
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
}