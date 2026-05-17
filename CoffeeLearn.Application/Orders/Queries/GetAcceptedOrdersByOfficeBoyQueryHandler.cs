using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Orders.Common;
using CoffeeLearn.Application.Orders.DTOs;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetAcceptedOrdersByOfficeBoyQueryHandler : IRequestHandler<GetAcceptedOrdersByOfficeBoyQuery, PagedResult<OrderDto>>
{
	private readonly IApplicationDbContext _context;

	public GetAcceptedOrdersByOfficeBoyQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<PagedResult<OrderDto>> Handle(GetAcceptedOrdersByOfficeBoyQuery request, CancellationToken cancellationToken)
	{
		var query = _context.Orders
			.AsNoTracking()
			.IncludeOrderDetails()
			.Where(x => x.AcceptedByOfficeBoyId == request.OfficeBoyId && x.Status == OrderStatus.Accepted);

		query = OrderSortingHelper.ApplySorting(query, request.SortBy, request.SortDirection, "acceptedat");

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