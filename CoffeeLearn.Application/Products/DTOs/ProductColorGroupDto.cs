namespace CoffeeLearn.Application.Products.DTOs;

public class ProductColorGroupDto
{
	public string Color { get; set; } = string.Empty;
	public string? ImageUrl { get; set; }

	public int TotalStock { get; set; }

	public List<ProductSizeStockDto> Sizes { get; set; } = new();
}






