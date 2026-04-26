namespace CoffeeLearn.Application.Orders.DTOs;

public class OrderDto
{
	public int Id { get; set; }
	public Guid UserId { get; set; }
	public Guid FloorId { get; set; }
	public string Status { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
	public List<OrderItemDto> Items { get; set; } = new();
}