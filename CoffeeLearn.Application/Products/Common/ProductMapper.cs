using CoffeeLearn.Application.Products.DTOs;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Products.Common;

public static class ProductMapper
{
	public static ProductDto ToDto(CoffeeLearn.Domain.Entities.Product product)
	{
		var activeVariants = product.Variants
			.Where(x => x.IsActive && !x.IsDeleted)
			.ToList();

		return new ProductDto
		{
			Id = product.Id,
			Name = product.Name,
			Quantity = product.Quantity,
			Price = product.Price,
			IsActive = product.IsActive,
			Description = product.Description,
			ImageUrl = product.ImageUrl,
			CategoryId = product.CategoryId,
			CategoryName = product.Category?.Name,
			BrandId = product.BrandId,
			BrandName = product.Brand?.Name,

			TotalVariantStock = activeVariants.Sum(x => x.Quantity),
			ActiveVariantCount = activeVariants.Count,

			AvailabilityStatus = GetAvailabilityStatus(product),
			CreatedAt = product.CreatedAt,
			UpdatedAt = product.UpdatedAt,
			IsDeleted = product.IsDeleted,
			DeletedAt = product.DeletedAt
		};
	}

	private static ProductAvailabilityStatus GetAvailabilityStatus(CoffeeLearn.Domain.Entities.Product product)
	{
		if (product.IsDeleted)
			return ProductAvailabilityStatus.Deleted;

		if (!product.IsActive)
			return ProductAvailabilityStatus.Inactive;

		var hasAvailableVariant = product.Variants
			.Any(x => x.IsActive && !x.IsDeleted && x.Quantity > 0);

		if (!hasAvailableVariant)
			return ProductAvailabilityStatus.OutOfStock;

		return ProductAvailabilityStatus.Active;
	}
}