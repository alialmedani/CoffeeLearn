using CoffeeLearn.Application.Products.DTOs;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Products.Common;

public static class ProductMapper
{
	public static ProductDto ToDto(Product product)
	{
		return new ProductDto
		{
			Id = product.Id,
			Name = product.Name,
			Quantity = product.Quantity,
			Price = product.Price,
			IsActive = product.IsActive,
			AvailabilityStatus = GetAvailabilityStatus(product),
			CreatedAt = product.CreatedAt,
			UpdatedAt = product.UpdatedAt
		};
	}

	private static string GetAvailabilityStatus(Product product)
	{
		if (!product.IsActive)
			return "Inactive";

		if (product.Quantity <= 0)
			return "OutOfStock";

		return "Active";
	}
}