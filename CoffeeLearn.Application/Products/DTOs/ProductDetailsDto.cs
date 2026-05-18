using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Products.DTOs;

public class ProductDetailsDto
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public decimal Price { get; set; }
	public string? Description { get; set; }
	public string? ImageUrl { get; set; }

	public LookupDto? Category { get; set; }
	public LookupDto? Brand { get; set; }

	public int TotalVariantStock { get; set; }
	public int ActiveVariantCount { get; set; }

	public ProductAvailabilityStatus AvailabilityStatus { get; set; }

	public List<ProductColorGroupDto> Colors { get; set; } = new();
}






