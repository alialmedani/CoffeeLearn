using CoffeeLearn.Application.Products.DTOs;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Products.Common;

public static class ProductVariantMapper
{
	public static ProductVariantDto ToDto(ProductVariant variant)
	{
		return new ProductVariantDto
		{
			Id = variant.Id,
			ProductId = variant.ProductId,
			ProductName = variant.Product?.Name ?? string.Empty,
			Color = variant.Color,
			Size = variant.Size,
			Quantity = variant.Quantity,
			Sku = variant.Sku,
			ImageUrl = variant.ImageUrl,
			IsActive = variant.IsActive,
			CreatedAt = variant.CreatedAt,
			UpdatedAt = variant.UpdatedAt
		};
	}
}