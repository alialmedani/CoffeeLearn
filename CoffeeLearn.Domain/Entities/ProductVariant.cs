using CoffeeLearn.Domain.Common;

namespace CoffeeLearn.Domain.Entities;

public class ProductVariant : BaseEntity
{
	public int ProductId { get; set; }

	public Product Product { get; set; } = default!;

	public string Color { get; set; } = string.Empty;

 	public int? SizeOptionId { get; set; }

	public SizeOption? SizeOption { get; set; }
	public int Quantity { get; set; }

	public string? Sku { get; set; }
	public string? ImageUrl { get; set; }

	public bool IsActive { get; set; } = true;

	public void DecreaseStock(int quantity)
	{
		if (quantity <= 0)
			throw new InvalidOperationException("Quantity must be greater than zero.");

		if (Quantity < quantity)
			throw new InvalidOperationException("Insufficient variant stock.");

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

	public void Activate()
	{
		IsActive = true;
		MarkAsUpdated();
	}

	public void Deactivate()
	{
		IsActive = false;
		MarkAsUpdated();
	}
}