using CoffeeLearn.Domain.Common;

namespace CoffeeLearn.Domain.Entities;

public class Product : BaseEntity
{
	public string Name { get; set; } = string.Empty;
	public int Quantity { get; set; }
	public decimal Price { get; set; }

	public void DecreaseStock(int quantity)
	{
		if (quantity <= 0)
			throw new InvalidOperationException("Quantity must be greater than zero.");

		if (Quantity < quantity)
			throw new InvalidOperationException("Insufficient stock.");

		Quantity -= quantity;
		MarkAsUpdated();
	}

	public void IncreaseStock(int quantity)
	{
		if (quantity <= 0)
			throw new InvalidOperationException("Quantity must be greater than zero.");

		Quantity += quantity;
		MarkAsUpdated();
	}
}