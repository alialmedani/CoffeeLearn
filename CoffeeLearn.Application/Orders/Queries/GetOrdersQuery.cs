using MediatR;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Orders.DTOs;

namespace CoffeeLearn.Application.Orders.Queries;

public class GetOrdersQuery : IRequest<PagedResult<OrderDto>>
{
	public string? Search { get; set; }

	public Guid? UserId { get; set; }
	public Guid? FloorId { get; set; }
	public Guid? AcceptedByOfficeBoyId { get; set; }

	public string? Status { get; set; }

	public int SkipCount { get; set; } = 0;
	public int? MaxResultCount { get; set; }

	public string? SortBy { get; set; }
	public string? SortDirection { get; set; } = "desc";
}





