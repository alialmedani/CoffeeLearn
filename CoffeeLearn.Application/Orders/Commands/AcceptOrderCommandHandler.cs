using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Exceptions;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Orders.DTOs;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Orders.Commands;

public class AcceptOrderCommandHandler : IRequestHandler<AcceptOrderCommand, OrderDto?>
{
	private readonly IApplicationDbContext _context;

	public AcceptOrderCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<OrderDto?> Handle(AcceptOrderCommand request, CancellationToken cancellationToken)
	{
		var order = await _context.Orders
			.Include(x => x.Items)
			.FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);

		if (order is null)
			throw new NotFoundException($"Order with id {request.OrderId} was not found.");

		if (order.Status != OrderStatus.Pending)
			throw new BusinessRuleException("Only pending orders can be accepted.");

		order.Status = OrderStatus.Accepted;
		order.AcceptedByOfficeBoyId = request.OfficeBoyId;
		order.AcceptedAt = DateTime.UtcNow;

		await _context.SaveChangesAsync(cancellationToken);

		var productNames = await _context.Products
			.AsNoTracking()
			.ToDictionaryAsync(x => x.Id, x => x.Name, cancellationToken);

		return new OrderDto
		{
			Id = order.Id,
			UserId = order.UserId,
			FloorId = order.FloorId,
			Status = order.Status.ToString(),
			AcceptedByOfficeBoyId = order.AcceptedByOfficeBoyId,
			CreatedAt = order.CreatedAt,
			AcceptedAt = order.AcceptedAt,
			CompletedAt = order.CompletedAt,
			Items = order.Items.Select(i => new OrderItemDto
			{
				ProductId = i.ProductId,
				ProductName = productNames.TryGetValue(i.ProductId, out var name) ? name : string.Empty,
				Quantity = i.Quantity,
				Price = i.Price
			}).ToList()
		};
	}
}