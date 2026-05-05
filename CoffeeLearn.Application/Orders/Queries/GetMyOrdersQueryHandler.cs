using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
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
		var productNames = await _context.Products
			.AsNoTracking()
			.ToDictionaryAsync(x => x.Id, x => x.Name, cancellationToken);

		var orders = await _context.Orders
			.AsNoTracking()
			.Include(x => x.Items)
			.Where(x => x.UserId == request.UserId)
			.OrderByDescending(x => x.CreatedAt)
			.ToListAsync(cancellationToken);

		return orders.Select(x => new OrderDto
		{
			Id = x.Id,
			UserId = x.UserId,
			FloorId = x.FloorId,
			Status = x.Status.ToString(),
			AcceptedByOfficeBoyId = x.AcceptedByOfficeBoyId,
			CreatedAt = x.CreatedAt,
			AcceptedAt = x.AcceptedAt,
			CompletedAt = x.CompletedAt,
			Items = x.Items.Select(i => new OrderItemDto
			{
				ProductId = i.ProductId,
				ProductName = productNames.TryGetValue(i.ProductId, out var name) ? name : string.Empty,
				Quantity = i.Quantity,
				Price = i.Price
			}).ToList()
		}).ToList();
	}
}