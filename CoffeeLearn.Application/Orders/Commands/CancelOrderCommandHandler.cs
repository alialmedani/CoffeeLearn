using MediatR;
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
			.Select(x => (
				x.ProductId,
				ProductVariantId: x.ProductVariantId!.Value,
				x.Quantity))
			.ToList();

		if (variantItems.Count > 0)
		{
			var variants = await OrderVariantStockHelper.GetVariantsForItemsOrThrowAsync(
				_context,
				variantItems,
				cancellationToken);

			OrderVariantStockHelper.RestoreVariantStock(
				variants,
				variantItems);
		}

		order.Cancel();

		await _context.SaveChangesAsync(cancellationToken);

		var productNames = await OrderQueryHelper.GetProductNamesAsync(_context, cancellationToken);

		return OrderMapper.ToDto(order, productNames);
	}
}





