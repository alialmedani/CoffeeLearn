using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Common.Exceptions;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Orders.Common;

public static class OrderVariantStockHelper
{
	public static List<(int ProductId, int ProductVariantId, int Quantity)> AggregateVariantItems(
		IEnumerable<(int ProductId, int ProductVariantId, int Quantity)> items)
	{
		return items
			.GroupBy(x => new { x.ProductId, x.ProductVariantId })
			.Select(g => (
				ProductId: g.Key.ProductId,
				ProductVariantId: g.Key.ProductVariantId,
				Quantity: g.Sum(x => x.Quantity)))
			.ToList();
	}

	public static async Task<List<ProductVariant>> GetVariantsForItemsOrThrowAsync(
		IApplicationDbContext context,
		IEnumerable<(int ProductId, int ProductVariantId, int Quantity)> items,
		CancellationToken cancellationToken)
	{
		var requestedItems = AggregateVariantItems(items);

		var variantIds = requestedItems
			.Select(x => x.ProductVariantId)
			.Distinct()
			.ToList();

		var variants = await context.ProductVariants
			.Include(x => x.Product)
			.Where(x => variantIds.Contains(x.Id))
			.ToListAsync(cancellationToken);

		if (variants.Count != variantIds.Count)
			throw new NotFoundException("One or more selected product variants do not exist.");

		return variants;
	}

	public static void EnsureVariantStockAvailability(
		List<ProductVariant> variants,
		IEnumerable<(int ProductId, int ProductVariantId, int Quantity)> items)
	{
		var requestedItems = AggregateVariantItems(items);

		foreach (var item in requestedItems)
		{
			var variant = variants.First(x => x.Id == item.ProductVariantId);

			if (variant.ProductId != item.ProductId)
				throw new BusinessRuleException("Product variant does not belong to the selected product.");

			if (!variant.IsActive)
			{
				throw new BusinessRuleException(
					$"Product variant '{variant.Color} / {ProductVariantDisplayHelper.GetSizeName(variant)}' is inactive and cannot be ordered.");
			}

			if (variant.Quantity < item.Quantity)
			{
				throw new BusinessRuleException(
					$"Insufficient stock for variant '{variant.Color} / {ProductVariantDisplayHelper.GetSizeName(variant)}'. Available: {variant.Quantity}, Requested: {item.Quantity}.");
			}
		}
	}

	public static void DeductVariantStock(
		List<ProductVariant> variants,
		IEnumerable<(int ProductId, int ProductVariantId, int Quantity)> items)
	{
		var requestedItems = AggregateVariantItems(items);

		foreach (var item in requestedItems)
		{
			var variant = variants.First(x => x.Id == item.ProductVariantId);
			variant.DecreaseStock(item.Quantity);
		}
	}

	public static void RestoreVariantStock(
		List<ProductVariant> variants,
		IEnumerable<(int ProductId, int ProductVariantId, int Quantity)> items)
	{
		var requestedItems = AggregateVariantItems(items);

		foreach (var item in requestedItems)
		{
			var variant = variants.First(x => x.Id == item.ProductVariantId);
			variant.IncreaseStock(item.Quantity);
		}
	}
}







