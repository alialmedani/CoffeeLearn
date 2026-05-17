using CoffeeLearn.Application.Orders.DTOs;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Orders.Common;

public static class OrderMapper
{
	public static OrderDto ToDto(Order order, Dictionary<int, string> productNames)
	{
		return new OrderDto
		{
			Id = order.Id,
			UserId = order.UserId,
			FloorId = order.FloorId,
			Status = order.Status.ToString(),
			AcceptedByOfficeBoyId = order.AcceptedByOfficeBoyId,
			CreatedAt = order.CreatedAt,
			UpdatedAt = order.UpdatedAt,
			AcceptedAt = order.AcceptedAt,
			CompletedAt = order.CompletedAt,
			CancelledAt = order.CancelledAt,
			Items = order.Items.Select(i => new OrderItemDto
			{
				ProductId = i.ProductId,
				ProductName = productNames.TryGetValue(i.ProductId, out var name) ? name : string.Empty,

				ProductVariantId = i.ProductVariantId,
				VariantColor = i.ProductVariant?.Color,
				VariantSize = i.ProductVariant?.Size,
				VariantSku = i.ProductVariant?.Sku,

				Quantity = i.Quantity,
				Price = i.Price
			}).ToList()
		};
	}
}