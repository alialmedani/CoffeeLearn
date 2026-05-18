using MediatR;
using CoffeeLearn.Application.Common.Exceptions;
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
		if (request.Items.Any(x => !x.ProductVariantId.HasValue))
		{
			throw new BusinessRuleException("ProductVariantId is required for all order items.");
		}

		var requestedProductItems = request.Items
			.Select(x => (x.ProductId, x.Quantity))
			.ToList();

		var products = await OrderStockHelper.GetProductsForItemsOrThrowAsync(
			_context,
			requestedProductItems,
			cancellationToken);

		var productsById = products.ToDictionary(x => x.Id);

		foreach (var item in request.Items)
		{
			var product = productsById[item.ProductId];

			if (!product.IsActive)
			{
				throw new BusinessRuleException(
					$"Product '{product.Name}' is inactive and cannot be ordered.");
			}
		}

		var variantItems = request.Items
			.Select(x => (
				x.ProductId,
				ProductVariantId: x.ProductVariantId!.Value,
				x.Quantity))
			.ToList();

		var variants = await OrderVariantStockHelper.GetVariantsForItemsOrThrowAsync(
			_context,
			variantItems,
			cancellationToken);

		OrderVariantStockHelper.EnsureVariantStockAvailability(
			variants,
			variantItems);

		OrderVariantStockHelper.DeductVariantStock(
			variants,
			variantItems);

		var variantsById = variants.ToDictionary(x => x.Id);

		var order = new Order
		{
			UserId = request.UserId,
			FloorId = request.FloorId,
			Status = OrderStatus.Pending
		};

		foreach (var item in request.Items)
		{
			var product = productsById[item.ProductId];
			var variant = variantsById[item.ProductVariantId!.Value];

			order.Items.Add(new OrderItem
			{
				ProductId = product.Id,
				ProductVariantId = variant.Id,
				ProductVariant = variant,
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




