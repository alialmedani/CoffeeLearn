using CoffeeLearn.Domain.Common;

namespace CoffeeLearn.Domain.Entities;

public class OrderItem : BaseEntity
{
	public int OrderId { get; set; }
	public Order Order { get; set; } = default!;

	public int ProductId { get; set; }

	public int Quantity { get; set; }

	public decimal Price { get; set; }
}