using MediatR;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetMyOrdersQuery : IRequest<List<OrderDto>>
{
	public Guid UserId { get; set; }
	public string? SortBy { get; set; }
	public string? SortDirection { get; set; } = "desc";
}