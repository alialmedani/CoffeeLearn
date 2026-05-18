using MediatR;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Orders.Common;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Commands;

public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommand, OrderDto?>
{
	private readonly IApplicationDbContext _context;

	public CompleteOrderCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<OrderDto?> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
	{
		var order = await OrderQueryHelper.GetOrderWithItemsOrThrowAsync(
			_context,
			request.OrderId,
			cancellationToken);

		OrderRules.EnsureAcceptedForComplete(order);

		order.Complete();

		await _context.SaveChangesAsync(cancellationToken);

		var productNames = await OrderQueryHelper.GetProductNamesAsync(_context, cancellationToken);

		return OrderMapper.ToDto(order, productNames);
	}
}




