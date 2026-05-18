using CoffeeLearn.Application.Brands.DTOs;
using MediatR;

namespace CoffeeLearn.Application.Brands.Commands;

public class UpdateBrandCommand : IRequest<BrandDto?>
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
}




