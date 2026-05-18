using CoffeeLearn.Application.Categories.DTOs;
using MediatR;

namespace CoffeeLearn.Application.Categories.Commands;

public class UpdateCategoryCommand : IRequest<CategoryDto?>
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }

 }





