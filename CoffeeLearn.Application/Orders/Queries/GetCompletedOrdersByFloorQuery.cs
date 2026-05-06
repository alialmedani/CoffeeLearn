using MediatR;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetCompletedOrdersByFloorQuery : IRequest<List<OrderDto>>
{
	public Guid FloorId { get; set; }
	public string? SortBy { get; set; }
	public string? SortDirection { get; set; } = "desc";
}