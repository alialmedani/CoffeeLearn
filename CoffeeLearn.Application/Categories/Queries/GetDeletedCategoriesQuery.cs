using CoffeeLearn.Application.Categories.DTOs;
using CoffeeLearn.Application.Common.Models;
using MediatR;

namespace CoffeeLearn.Application.Categories.Queries;

public class GetDeletedCategoriesQuery : IRequest<PagedResult<CategoryDto>>
{
	public int SkipCount { get; set; } = 0;
	public int? MaxResultCount { get; set; }

	public string? SortBy { get; set; }
	public string? SortDirection { get; set; } = "desc";
}





