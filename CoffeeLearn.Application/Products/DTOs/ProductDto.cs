using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Products.DTOs;

public class ProductDto
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public int Quantity { get; set; }
	public decimal Price { get; set; }

	public bool IsActive { get; set; }
	public bool IsDeleted { get; set; }
	public DateTime? DeletedAt { get; set; }
	public string? Description { get; set; }
	public string? ImageUrl { get; set; }

	public int? CategoryId { get; set; }
	public string? CategoryName { get; set; }

	public int? BrandId { get; set; }
	public string? BrandName { get; set; }

	public int TotalVariantStock { get; set; }
	public int ActiveVariantCount { get; set; }

	public ProductAvailabilityStatus AvailabilityStatus { get; set; }

	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
}