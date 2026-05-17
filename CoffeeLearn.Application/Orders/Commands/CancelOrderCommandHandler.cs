using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Exceptions;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Orders.Common;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Commands;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, OrderDto?>
{
	private readonly IApplicationDbContext _context;

	public CancelOrderCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<OrderDto?> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
	{
		var order = await OrderQueryHelper.GetOrderWithItemsOrThrowAsync(
			_context,
			request.OrderId,
			cancellationToken);

		OrderRules.EnsureCanCancel(order);

		var productOnlyItems = order.Items
			.Where(x => !x.ProductVariantId.HasValue)
			.Select(x => (x.ProductId, x.Quantity))
			.ToList();

		if (productOnlyItems.Count > 0)
		{
			var products = await OrderStockHelper.GetProductsForItemsOrThrowAsync(
				_context,
				productOnlyItems,
				cancellationToken);

			OrderStockHelper.RestoreStock(products, productOnlyItems);
		}

		var variantItems = order.Items
			.Where(x => x.ProductVariantId.HasValue)
			.GroupBy(x => x.ProductVariantId!.Value)
			.Select(g => new
			{
				ProductVariantId = g.Key,
				Quantity = g.Sum(x => x.Quantity)
			})
			.ToList();

		if (variantItems.Count > 0)
		{
			var variantIds = variantItems
				.Select(x => x.ProductVariantId)
				.ToList();

			var variants = await _context.ProductVariants
				.Where(x => variantIds.Contains(x.Id))
				.ToListAsync(cancellationToken);

			if (variants.Count != variantIds.Count)
				throw new NotFoundException("One or more product variants do not exist.");

			foreach (var item in variantItems)
			{
				var variant = variants.First(x => x.Id == item.ProductVariantId);
				variant.IncreaseStock(item.Quantity);
			}
		}

		order.Cancel();

		await _context.SaveChangesAsync(cancellationToken);

		var productNames = await OrderQueryHelper.GetProductNamesAsync(_context, cancellationToken);

		return OrderMapper.ToDto(order, productNames);
	}
}