using MediatR;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetOrdersByFloorQuery : IRequest<List<OrderDto>>
{
	public Guid FloorId { get; set; }
}