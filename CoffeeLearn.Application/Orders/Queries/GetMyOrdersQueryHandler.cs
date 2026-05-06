using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Orders.Common;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQuery, List<OrderDto>>
{
	private readonly IApplicationDbContext _context;

	public GetMyOrdersQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<OrderDto>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
	{
		var productNames = await OrderQueryHelper.GetProductNamesAsync(_context, cancellationToken);

		var orders = await _context.Orders
			.AsNoTracking()
			.Include(x => x.Items)
			.Where(x => x.UserId == request.UserId)
			.OrderByDescending(x => x.CreatedAt)
			.ToListAsync(cancellationToken);

		return orders.Select(x => OrderMapper.ToDto(x, productNames)).ToList();
	}
}