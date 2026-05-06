using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Orders.Common;
using CoffeeLearn.Application.Orders.DTOs;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetAcceptedOrdersByOfficeBoyQueryHandler : IRequestHandler<GetAcceptedOrdersByOfficeBoyQuery, List<OrderDto>>
{
	private readonly IApplicationDbContext _context;

	public GetAcceptedOrdersByOfficeBoyQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<OrderDto>> Handle(GetAcceptedOrdersByOfficeBoyQuery request, CancellationToken cancellationToken)
	{
		var productNames = await OrderQueryHelper.GetProductNamesAsync(_context, cancellationToken);

		var orders = await _context.Orders
			.AsNoTracking()
			.Include(x => x.Items)
			.Where(x => x.AcceptedByOfficeBoyId == request.OfficeBoyId && x.Status == OrderStatus.Accepted)
			.OrderByDescending(x => x.AcceptedAt)
			.ToListAsync(cancellationToken);

		return orders.Select(x => OrderMapper.ToDto(x, productNames)).ToList();
	}
}