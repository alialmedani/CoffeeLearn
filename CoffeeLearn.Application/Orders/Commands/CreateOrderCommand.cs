using MediatR;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Commands;

public class CreateOrderCommand : IRequest<OrderDto>
{
	public Guid UserId { get; set; }
	public Guid FloorId { get; set; }

	public List<CreateOrderItemDto> Items { get; set; } = new();
}





