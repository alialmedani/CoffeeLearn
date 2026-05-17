using MediatR;
using Microsoft.EntityFrameworkCore;
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

		var productOnlyItems = request.Items
			.Where(x => !x.ProductVariantId.HasValue)
			.Select(x => (x.ProductId, x.Quantity))
			.ToList();

		if (productOnlyItems.Count > 0)
		{
			OrderStockHelper.EnsureStockAvailability(products, productOnlyItems);
			OrderStockHelper.DeductStock(products, productOnlyItems);
		}

		var variantRequestItems = request.Items
			.Where(x => x.ProductVariantId.HasValue)
			.Select(x => new
			{
				x.ProductId,
				ProductVariantId = x.ProductVariantId!.Value,
				x.Quantity
			})
			.ToList();

		var variants = new List<ProductVariant>();

		if (variantRequestItems.Count > 0)
		{
			var variantIds = variantRequestItems
				.Select(x => x.ProductVariantId)
				.Distinct()
				.ToList();

			variants = await _context.ProductVariants
				.Include(x => x.Product)
				.Where(x => variantIds.Contains(x.Id))
				.ToListAsync(cancellationToken);

			if (variants.Count != variantIds.Count)
				throw new NotFoundException("One or more selected product variants do not exist.");

			var requestedVariants = variantRequestItems
				.GroupBy(x => new { x.ProductId, x.ProductVariantId })
				.Select(g => new
				{
					g.Key.ProductId,
					g.Key.ProductVariantId,
					Quantity = g.Sum(x => x.Quantity)
				})
				.ToList();

			foreach (var item in requestedVariants)
			{
				var variant = variants.First(x => x.Id == item.ProductVariantId);

				if (variant.ProductId != item.ProductId)
					throw new BusinessRuleException("Product variant does not belong to the selected product.");

				if (!variant.IsActive)
				{
					throw new BusinessRuleException(
						$"Product variant '{variant.Color} / {variant.Size}' is inactive and cannot be ordered.");
				}

				if (variant.Quantity < item.Quantity)
				{
					throw new BusinessRuleException(
						$"Insufficient stock for variant '{variant.Color} / {variant.Size}'. Available: {variant.Quantity}, Requested: {item.Quantity}.");
				}
			}

			foreach (var item in requestedVariants)
			{
				var variant = variants.First(x => x.Id == item.ProductVariantId);
				variant.DecreaseStock(item.Quantity);
			}
		}

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

			ProductVariant? variant = null;

			if (item.ProductVariantId.HasValue)
			{
				variant = variantsById[item.ProductVariantId.Value];
			}

			order.Items.Add(new OrderItem
			{
				ProductId = product.Id,
				ProductVariantId = item.ProductVariantId,
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