using CoffeeLearn.Application.Brands.DTOs;
using CoffeeLearn.Application.Common.Models;
using MediatR;

namespace CoffeeLearn.Application.Brands.Queries;

public class GetDeletedBrandsQuery : IRequest<PagedResult<BrandDto>>
{
	public int SkipCount { get; set; } = 0;
	public int? MaxResultCount { get; set; }

	public string? SortBy { get; set; }
	public string? SortDirection { get; set; } = "desc";
}




