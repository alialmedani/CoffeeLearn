using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Orders.Common;

public static class OrderQueryableExtensions
{
	public static IQueryable<Order> IncludeOrderDetails(this IQueryable<Order> query)
	{
		return query
			.Include(x => x.Items)
				.ThenInclude(x => x.ProductVariant);
	}
}




