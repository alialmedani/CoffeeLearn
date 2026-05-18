using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Common;

public static class ProductVariantMapper
{
	public static ProductVariantDto ToDto(CoffeeLearn.Domain.Entities.ProductVariant variant)
	{
		return new ProductVariantDto
		{
			Id = variant.Id,
			ProductId = variant.ProductId,
			ProductName = variant.Product?.Name ?? string.Empty,
			Color = variant.Color,
			SizeOptionId = variant.SizeOptionId,
			SizeOptionName = ProductVariantDisplayHelper.GetSizeName(variant),
			Quantity = variant.Quantity,
			Sku = variant.Sku,
			ImageUrl = variant.ImageUrl,
			IsActive = variant.IsActive,
			CreatedAt = variant.CreatedAt,
			UpdatedAt = variant.UpdatedAt
		};
	}
}


