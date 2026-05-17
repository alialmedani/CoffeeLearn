using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Orders.Common;
using CoffeeLearn.Application.Orders.DTOs;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, PagedResult<OrderDto>>
{
	private readonly IApplicationDbContext _context;

	public GetOrdersQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<PagedResult<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
	{
		var query = _context.Orders
			.AsNoTracking()
			.AsQueryable();

		if (request.UserId.HasValue)
		{
			query = query.Where(x => x.UserId == request.UserId.Value);
		}

		if (request.FloorId.HasValue)
		{
			query = query.Where(x => x.FloorId == request.FloorId.Value);
		}

		if (request.AcceptedByOfficeBoyId.HasValue)
		{
			query = query.Where(x => x.AcceptedByOfficeBoyId == request.AcceptedByOfficeBoyId.Value);
		}

		if (!string.IsNullOrWhiteSpace(request.Status) &&
			Enum.TryParse<OrderStatus>(request.Status, true, out var parsedStatus))
		{
			query = query.Where(x => x.Status == parsedStatus);
		}

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var search = request.Search.Trim().ToLower();

			var matchingProductIds = await _context.Products
				.AsNoTracking()
				.Where(x => x.Name.ToLower().Contains(search))
				.Select(x => x.Id)
				.ToListAsync(cancellationToken);

			query = query.Where(x => x.Items.Any(i => matchingProductIds.Contains(i.ProductId)));
		}

		query = OrderSortingHelper.ApplySorting(
			query,
			request.SortBy,
			request.SortDirection);

		var totalCount = await query.CountAsync(cancellationToken);

		var orders = await query
			.IncludeOrderDetails()
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