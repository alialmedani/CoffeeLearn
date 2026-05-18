using CoffeeLearn.Application.Products.DTOs;
using MediatR;

namespace CoffeeLearn.Application.Products.Commands;

public class CreateProductWithVariantsCommand : IRequest<ProductDto>
{
	public string Name { get; set; } = string.Empty;
	public decimal Price { get; set; }

	public string? Description { get; set; }
	public string? ImageUrl { get; set; }

	public int? CategoryId { get; set; }
	public int? BrandId { get; set; }

	public List<CreateProductWithVariantItemDto> Variants { get; set; } = new();
}






