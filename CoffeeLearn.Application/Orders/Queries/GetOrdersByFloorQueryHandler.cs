using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Orders.Common;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetOrdersByFloorQueryHandler : IRequestHandler<GetOrdersByFloorQuery, PagedResult<OrderDto>>
{
	private readonly IApplicationDbContext _context;

	public GetOrdersByFloorQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<PagedResult<OrderDto>> Handle(GetOrdersByFloorQuery request, CancellationToken cancellationToken)
	{
		var query = _context.Orders
			.AsNoTracking()
			.Include(x => x.Items)
			.Where(x => x.FloorId == request.FloorId);

		query = OrderSortingHelper.ApplySorting(query, request.SortBy, request.SortDirection, "createdat");

		var totalCount = await query.CountAsync(cancellationToken);

		var orders = await query
			.Skip((request.PageNumber - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToListAsync(cancellationToken);

		var productNames = await OrderQueryHelper.GetProductNamesAsync(_context, cancellationToken);

		return new PagedResult<OrderDto>
		{
			Items = orders.Select(x => OrderMapper.ToDto(x, productNames)).ToList(),
			TotalCount = totalCount,
			PageNumber = request.PageNumber,
			PageSize = request.PageSize
		};
	}
}