using CoffeeLearn.Application.Common.Exceptions;
using CoffeeLearn.Domain.Entities;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Orders.Common;

public static class OrderRules
{
	public static void EnsurePendingForAccept(Order order)
	{
		if (order.Status == OrderStatus.Accepted)
			throw new BusinessRuleException("Order is already accepted.");

		if (order.Status == OrderStatus.Completed)
			throw new BusinessRuleException("Completed orders cannot be accepted.");

		if (order.Status == OrderStatus.Cancelled)
			throw new BusinessRuleException("Cancelled orders cannot be accepted.");

		if (order.Status != OrderStatus.Pending)
			throw new BusinessRuleException("Only pending orders can be accepted.");
	}

	public static void EnsureAcceptedForComplete(Order order)
	{
		if (order.Status == OrderStatus.Completed)
			throw new BusinessRuleException("Order is already completed.");

		if (order.Status == OrderStatus.Cancelled)
			throw new BusinessRuleException("Cancelled orders cannot be completed.");

		if (order.Status != OrderStatus.Accepted)
			throw new BusinessRuleException("Only accepted orders can be completed.");
	}

	public static void EnsureCanCancel(Order order)
	{
		if (order.Status == OrderStatus.Cancelled)
			throw new BusinessRuleException("Order is already cancelled.");

		if (order.Status == OrderStatus.Completed)
			throw new BusinessRuleException("Completed orders cannot be cancelled.");
	}
}