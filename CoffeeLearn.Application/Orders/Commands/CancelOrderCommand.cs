using MediatR;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Commands;

public class CancelOrderCommand : IRequest<OrderDto?>
{
	public int OrderId { get; set; }
}