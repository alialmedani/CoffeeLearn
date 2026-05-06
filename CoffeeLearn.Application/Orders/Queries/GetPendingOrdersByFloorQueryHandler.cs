using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Orders.Common;
using CoffeeLearn.Application.Orders.DTOs;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetPendingOrdersByFloorQueryHandler : IRequestHandler<GetPendingOrdersByFloorQuery, List<OrderDto>>
{
	private readonly IApplicationDbContext _context;

	public GetPendingOrdersByFloorQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<OrderDto>> Handle(GetPendingOrdersByFloorQuery request, CancellationToken cancellationToken)
	{
		var productNames = await OrderQueryHelper.GetProductNamesAsync(_context, cancellationToken);

		var query = _context.Orders
			.AsNoTracking()
			.Include(x => x.Items)
			.Where(x => x.FloorId == request.FloorId && x.Status == OrderStatus.Pending);

		query = OrderSortingHelper.ApplySorting(query, request.SortBy, request.SortDirection, "createdat");

		var orders = await query.ToListAsync(cancellationToken);

		return orders.Select(x => OrderMapper.ToDto(x, productNames)).ToList();
	}
}