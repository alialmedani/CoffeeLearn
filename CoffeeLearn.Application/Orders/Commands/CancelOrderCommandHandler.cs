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

		var requestedItems = order.Items
			.Select(x => (x.ProductId, x.Quantity))
			.ToList();

		var products = await OrderStockHelper.GetProductsForItemsOrThrowAsync(
			_context,
			requestedItems,
			cancellationToken);

		OrderStockHelper.RestoreStock(products, requestedItems);

		order.Cancel();

		await _context.SaveChangesAsync(cancellationToken);

		var productNames = await OrderQueryHelper.GetProductNamesAsync(_context, cancellationToken);

		return OrderMapper.ToDto(order, productNames);
	}
}