namespace CoffeeLearn.Application.Products.DTOs;

public class ProductDto
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public int Quantity { get; set; }
	public decimal Price { get; set; }

	public bool IsActive { get; set; }
	public string AvailabilityStatus { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }

}