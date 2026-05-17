namespace CoffeeLearn.Application.Reports.DTOs;

public class LowStockVariantDto
{
	public int ProductId { get; set; }
	public string ProductName { get; set; } = string.Empty;

	public int? CategoryId { get; set; }
	public string? CategoryName { get; set; }

	public int? BrandId { get; set; }
	public string? BrandName { get; set; }

	public int ProductVariantId { get; set; }
	public string Color { get; set; } = string.Empty;
	public string Size { get; set; } = string.Empty;
	public string? Sku { get; set; }

	public int Quantity { get; set; }
	public bool IsActive { get; set; }
}