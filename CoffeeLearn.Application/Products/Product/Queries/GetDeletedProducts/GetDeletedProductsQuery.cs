using MediatR;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Queries;

public class GetDeletedProductsQuery : IRequest<PagedResult<ProductDto>>
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;

	public string? SortBy { get; set; }
	public string? SortDirection { get; set; } = "desc";

}