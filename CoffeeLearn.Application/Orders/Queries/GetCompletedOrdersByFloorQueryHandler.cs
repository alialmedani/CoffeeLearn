using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Orders.Common;
using CoffeeLearn.Application.Orders.DTOs;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetCompletedOrdersByFloorQueryHandler : IRequestHandler<GetCompletedOrdersByFloorQuery, List<OrderDto>>
{
	private readonly IApplicationDbContext _context;

	public GetCompletedOrdersByFloorQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<OrderDto>> Handle(GetCompletedOrdersByFloorQuery request, CancellationToken cancellationToken)
	{
		var productNames = await OrderQueryHelper.GetProductNamesAsync(_context, cancellationToken);

		var orders = await _context.Orders
			.AsNoTracking()
			.Include(x => x.Items)
			.Where(x => x.FloorId == request.FloorId && x.Status == OrderStatus.Completed)
			.OrderByDescending(x => x.CompletedAt)
			.ToListAsync(cancellationToken);

		return orders.Select(x => OrderMapper.ToDto(x, productNames)).ToList();
	}
}