using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Orders.Common;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderDto>>
{
	private readonly IApplicationDbContext _context;

	public GetOrdersQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
	{
		var productNames = await OrderQueryHelper.GetProductNamesAsync(_context, cancellationToken);

		var orders = await _context.Orders
			.AsNoTracking()
			.Include(x => x.Items)
			.OrderByDescending(x => x.CreatedAt)
			.ToListAsync(cancellationToken);

		return orders.Select(x => OrderMapper.ToDto(x, productNames)).ToList();
	}
}