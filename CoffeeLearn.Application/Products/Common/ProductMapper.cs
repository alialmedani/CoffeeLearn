using CoffeeLearn.Application.Products.DTOs;
using CoffeeLearn.Domain.Entities;
using CoffeeLearn.Domain.Enums;

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
			Description = product.Description,
			ImageUrl = product.ImageUrl,
			AvailabilityStatus = GetAvailabilityStatus(product),
			CreatedAt = product.CreatedAt,
			UpdatedAt = product.UpdatedAt,
			IsDeleted = product.IsDeleted,
			DeletedAt = product.DeletedAt
		};
	}

	private static ProductAvailabilityStatus GetAvailabilityStatus(Product product)
	{
		if (product.IsDeleted)
			return ProductAvailabilityStatus.Deleted;

		if (!product.IsActive)
			return ProductAvailabilityStatus.Inactive;

		if (product.Quantity <= 0)
			return ProductAvailabilityStatus.OutOfStock;

		return ProductAvailabilityStatus.Active;
	}
}