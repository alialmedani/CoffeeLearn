using MediatR;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Queries;

public class GetDeletedProductsQuery : IRequest<PagedResult<ProductDto>>
{
	public int SkipCount { get; set; } = 0;
	public int? MaxResultCount { get; set; }

	public string? SortBy { get; set; }
	public string? SortDirection { get; set; } = "desc";
}




