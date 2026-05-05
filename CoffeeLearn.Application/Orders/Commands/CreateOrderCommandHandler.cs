using CoffeeLearn.Application.Common.Exceptions;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Orders.DTOs;
using CoffeeLearn.Domain.Entities;
using CoffeeLearn.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Orders.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
	private readonly IApplicationDbContext _context;

	public CreateOrderCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
	{
		var productIds = request.Items
			.Select(x => x.ProductId)
			.Distinct()
			.ToList();

		var products = await _context.Products
			.Where(x => productIds.Contains(x.Id))
			.ToListAsync(cancellationToken);

		if (products.Count != productIds.Count)
		{
			throw new NotFoundException("One or more selected products do not exist.");
		}

		var order = new Order
		{
			UserId = request.UserId,
			FloorId = request.FloorId,
			Status = OrderStatus.Pending,
			CreatedAt = DateTime.UtcNow
		};

		foreach (var item in request.Items)
		{
			var product = products.First(x => x.Id == item.ProductId);

			order.Items.Add(new OrderItem
			{
				ProductId = product.Id,
				Quantity = item.Quantity,
				Price = product.Price
			});
		}

		_context.Orders.Add(order);
		await _context.SaveChangesAsync(cancellationToken);

		return new OrderDto
		{
			Id = order.Id,
			UserId = order.UserId,
			FloorId = order.FloorId,
			Status = order.Status.ToString(),
			CreatedAt = order.CreatedAt,
			Items = order.Items.Select(x => new OrderItemDto
			{
				ProductId = x.ProductId,
				ProductName = products.First(p => p.Id == x.ProductId).Name,
				Quantity = x.Quantity,
				Price = x.Price
			}).ToList()
		};
	}
}