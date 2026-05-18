namespace CoffeeLearn.Application.Orders.DTOs;

public class OrderItemDto
{
	public int ProductId { get; set; }
	public string ProductName { get; set; } = string.Empty;

	public int? ProductVariantId { get; set; }
	public string? VariantColor { get; set; }
	public string? VariantSize { get; set; }
	public string? VariantSku { get; set; }

	public int Quantity { get; set; }

	public decimal Price { get; set; }
}




