using CoffeeLearn.Application.Brands.DTOs;
using CoffeeLearn.Application.Common.Models;
using MediatR;

namespace CoffeeLearn.Application.Brands.Queries;

public class GetBrandsQuery : IRequest<PagedResult<BrandDto>>
{
	public string? Search { get; set; }
	public bool? IsActive { get; set; }

	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;

	public string? SortBy { get; set; }
	public string? SortDirection { get; set; } = "asc";
}