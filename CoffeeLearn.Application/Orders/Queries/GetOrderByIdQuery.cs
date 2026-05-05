using MediatR;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetOrderByIdQuery : IRequest<OrderDto?>
{
	public int Id { get; set; }
}