using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
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
		return await _context.Orders
			.AsNoTracking()
			.Include(x => x.Items)
			.ThenInclude(x => x.Order)
			.Where(x => x.Id == request.Id)
			.Select(x => new OrderDto
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
					ProductName = _context.Products
						.Where(p => p.Id == i.ProductId)
						.Select(p => p.Name)
						.FirstOrDefault() ?? string.Empty,
					Quantity = i.Quantity,
					Price = i.Price
				}).ToList()
			})
			.FirstOrDefaultAsync(cancellationToken);
	}
}