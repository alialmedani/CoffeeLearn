using MediatR;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetOrdersQuery : IRequest<List<OrderDto>>
{
}