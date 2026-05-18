using CoffeeLearn.Application.Brands.DTOs;
using MediatR;

namespace CoffeeLearn.Application.Brands.Commands;

public class CreateBrandCommand : IRequest<BrandDto>
{
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
}





