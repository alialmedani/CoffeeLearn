using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Orders.Common;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
	private readonly IApplicationDbContext _context;

	public GetOrderByIdQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
	{
		var order = await _context.Orders
			.AsNoTracking()
			.Include(x => x.Items)
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (order is null)
			return null;

		var productNames = await OrderQueryHelper.GetProductNamesAsync(_context, cancellationToken);

		return OrderMapper.ToDto(order, productNames);
	}
}