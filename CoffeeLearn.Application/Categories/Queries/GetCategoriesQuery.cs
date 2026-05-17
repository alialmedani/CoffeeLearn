using CoffeeLearn.Application.Categories.DTOs;
using CoffeeLearn.Application.Common.Models;
using MediatR;

namespace CoffeeLearn.Application.Categories.Queries;

public class GetCategoriesQuery : IRequest<PagedResult<CategoryDto>>
{
	public string? Search { get; set; }
	public bool? IsActive { get; set; }

	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;

	public string? SortBy { get; set; }
	public string? SortDirection { get; set; } = "asc";
}