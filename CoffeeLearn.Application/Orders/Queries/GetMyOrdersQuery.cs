using MediatR;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Orders.Common;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetMyOrdersQuery : OrderListQueryBase, IRequest<PagedResult<OrderDto>>
{
	public Guid UserId { get; set; }
}




