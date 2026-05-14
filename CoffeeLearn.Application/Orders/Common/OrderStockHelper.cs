using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Exceptions;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Orders.Common;

public static class OrderStockHelper
{
	public static List<(int ProductId, int Quantity)> AggregateItems(IEnumerable<(int ProductId, int Quantity)> items)
	{
		return items
			.GroupBy(x => x.ProductId)
			.Select(g => (ProductId: g.Key, Quantity: g.Sum(x => x.Quantity)))
			.ToList();
	}

	public static async Task<List<Product>> GetProductsForItemsOrThrowAsync(
		IApplicationDbContext context,
		IEnumerable<(int ProductId, int Quantity)> items,
		CancellationToken cancellationToken)
	{
		var requestedItems = AggregateItems(items);

		var productIds = requestedItems
			.Select(x => x.ProductId)
			.ToList();

		var products = await context.Products
			.Where(x => productIds.Contains(x.Id))
			.ToListAsync(cancellationToken);

		if (products.Count != productIds.Count)
			throw new NotFoundException("One or more selected products do not exist.");

		return products;
	}

	public static void EnsureStockAvailability(
		List<Product> products,
		IEnumerable<(int ProductId, int Quantity)> items)
	{
		var requestedItems = AggregateItems(items);

		foreach (var item in requestedItems)
		{
			var product = products.First(x => x.Id == item.ProductId);

			if (product.Quantity < item.Quantity)
			{
				throw new BusinessRuleException(
					$"Insufficient stock for product '{product.Name}'. Available: {product.Quantity}, Requested: {item.Quantity}.");
			}
		}
	}

	public static void DeductStock(
		List<Product> products,
		IEnumerable<(int ProductId, int Quantity)> items)
	{
		var requestedItems = AggregateItems(items);

		foreach (var item in requestedItems)
		{
			var product = products.First(x => x.Id == item.ProductId);
			product.DecreaseStock(item.Quantity);
		}
	}

	public static void RestoreStock(
		List<Product> products,
		IEnumerable<(int ProductId, int Quantity)> items)
	{
		var requestedItems = AggregateItems(items);

		foreach (var item in requestedItems)
		{
			var product = products.First(x => x.Id == item.ProductId);
			product.IncreaseStock(item.Quantity);
		}
	}
}