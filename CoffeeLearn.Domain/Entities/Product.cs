using CoffeeLearn.Domain.Common;

namespace CoffeeLearn.Domain.Entities;

public class Product : BaseEntity
{
	public string Name { get; set; } = string.Empty;
	public int Quantity { get; set; }
	public decimal Price { get; set; }
	public bool IsActive { get; set; } = true;
	public string? Description { get; set; }
	public string? ImageUrl { get; set; }

	public int? CategoryId { get; set; }
	public Category? Category { get; set; }

	public List<ProductVariant> Variants { get; set; } = new();

	public void Activate()
	{
		IsActive = true;
	}

	public void Deactivate()
	{
		IsActive = false;
	}

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