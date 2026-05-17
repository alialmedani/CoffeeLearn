using CoffeeLearn.Application.Categories.DTOs;
using CoffeeLearn.Application.Common.Models;
using MediatR;

namespace CoffeeLearn.Application.Categories.Queries;

public class GetDeletedCategoriesQuery : IRequest<PagedResult<CategoryDto>>
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;

	public string? SortBy { get; set; }
	public string? SortDirection { get; set; } = "desc";
}