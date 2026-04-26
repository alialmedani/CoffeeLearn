using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Domain.Entities;

public class Order
{
	public int Id { get; set; }

	public Guid UserId { get; set; }
	public Guid FloorId { get; set; }

	public OrderStatus Status { get; set; } = OrderStatus.Pending;

	public Guid? AcceptedByOfficeBoyId { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime? AcceptedAt { get; set; }
	public DateTime? CompletedAt { get; set; }

	public List<OrderItem> Items { get; set; } = new();
}