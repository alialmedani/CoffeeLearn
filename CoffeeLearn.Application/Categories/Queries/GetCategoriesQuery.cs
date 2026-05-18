using CoffeeLearn.Application.Categories.DTOs;
using CoffeeLearn.Application.Common.Models;
using MediatR;

namespace CoffeeLearn.Application.Categories.Queries;

public class GetCategoriesQuery : IRequest<PagedResult<CategoryDto>>
{
	public string? Search { get; set; }
	public bool? IsActive { get; set; }

	public int SkipCount { get; set; } = 0;
	public int? MaxResultCount { get; set; }

	public string? SortBy { get; set; }
	public string? SortDirection { get; set; } = "asc";
}





