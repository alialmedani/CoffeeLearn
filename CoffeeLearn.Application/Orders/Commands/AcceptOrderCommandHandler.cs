using MediatR;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Orders.Common;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Commands;

public class AcceptOrderCommandHandler : IRequestHandler<AcceptOrderCommand, OrderDto?>
{
	private readonly IApplicationDbContext _context;

	public AcceptOrderCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<OrderDto?> Handle(AcceptOrderCommand request, CancellationToken cancellationToken)
	{
		var order = await OrderQueryHelper.GetOrderWithItemsOrThrowAsync(
			_context,
			request.OrderId,
			cancellationToken);

		OrderRules.EnsurePendingForAccept(order);

		order.Accept(request.OfficeBoyId);

		await _context.SaveChangesAsync(cancellationToken);

		var productNames = await OrderQueryHelper.GetProductNamesAsync(_context, cancellationToken);

		return OrderMapper.ToDto(order, productNames);
	}
}





