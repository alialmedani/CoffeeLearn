using MediatR;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Products.DTOs;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Products.Queries;

public class GetProductsQuery : IRequest<PagedResult<ProductDto>>
{
	public string? Search { get; set; }

	public decimal? MinPrice { get; set; }
	public decimal? MaxPrice { get; set; }

	public bool? IsActive { get; set; }
	public ProductAvailabilityStatus? AvailabilityStatus { get; set; }

	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;

	public string? SortBy { get; set; }
	public string? SortDirection { get; set; } = "asc";
}