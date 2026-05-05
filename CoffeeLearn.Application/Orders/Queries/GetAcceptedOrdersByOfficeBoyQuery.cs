using MediatR;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetAcceptedOrdersByOfficeBoyQuery : IRequest<List<OrderDto>>
{
	public Guid OfficeBoyId { get; set; }
}