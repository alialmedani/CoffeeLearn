using MediatR;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Orders.Common;
using CoffeeLearn.Application.Orders.DTOs;
using CoffeeLearn.Domain.Entities;
using CoffeeLearn.Domain.Enums;

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
		var requestedItems = request.Items
			.Select(x => (x.ProductId, x.Quantity))
			.ToList();

		var products = await OrderStockHelper.GetProductsForItemsOrThrowAsync(
			_context,
			requestedItems,
			cancellationToken);

		OrderStockHelper.EnsureStockAvailability(products, requestedItems);

		// الخصم صار هنا
		OrderStockHelper.DeductStock(products, requestedItems);

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

		var productNames = await OrderQueryHelper.GetProductNamesAsync(_context, cancellationToken);

		return OrderMapper.ToDto(order, productNames);
	}
}