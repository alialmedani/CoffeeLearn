using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Exceptions;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Orders.Common;

public static class OrderQueryHelper
{
	public static async Task<Order> GetOrderWithItemsOrThrowAsync(
		IApplicationDbContext context,
		int orderId,
		CancellationToken cancellationToken)
	{
		var order = await context.Orders
			.IncludeOrderDetails()
			.FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

		if (order is null)
			throw new NotFoundException($"Order with id {orderId} was not found.");

		return order;
	}

	public static async Task<Dictionary<int, string>> GetProductNamesAsync(
		IApplicationDbContext context,
		CancellationToken cancellationToken)
	{
		return await context.Products
			.AsNoTracking()
			.ToDictionaryAsync(x => x.Id, x => x.Name, cancellationToken);
	}
}





