namespace CoffeeLearn.Application.Products.DTOs;

public class CreateProductWithVariantItemDto
{
	public string Color { get; set; } = string.Empty;
	public string Size { get; set; } = string.Empty;
	public int Quantity { get; set; }
	public string? Sku { get; set; }
	public string? ImageUrl { get; set; }
	public bool IsActive { get; set; } = true;
}