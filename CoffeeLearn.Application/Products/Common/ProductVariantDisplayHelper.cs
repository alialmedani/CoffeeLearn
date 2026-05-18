using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Products.Common;

public static class ProductVariantDisplayHelper
{
	public static string GetSizeName(ProductVariant variant)
	{
		return variant.SizeOption?.Name ?? string.Empty;
	}
}
