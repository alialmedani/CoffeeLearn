using MediatR;
using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Products.DTOs;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Products.Queries;

public class GetProductsQuery : IRequest<PagedResult<ProductDto>>
{
	public string? Search { get; set; }

	public int? CategoryId { get; set; }

	public decimal? MinPrice { get; set; }
	public decimal? MaxPrice { get; set; }

	public bool? IsActive { get; set; }
	public ProductAvailabilityStatus? AvailabilityStatus { get; set; }

	public int SkipCount { get; set; } = 0;
	public int? MaxResultCount { get; set; }

	public string? SortBy { get; set; }
	public string? SortDirection { get; set; } = "asc";
	public int? BrandId { get; set; }
}






