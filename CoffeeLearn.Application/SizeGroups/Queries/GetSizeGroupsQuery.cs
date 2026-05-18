using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.SizeGroups.DTOs;
using MediatR;

namespace CoffeeLearn.Application.SizeGroups.Queries;

public class GetSizeGroupsQuery : IRequest<PagedResult<SizeGroupDto>>
{
	public int? CategoryId { get; set; }
	public string? Search { get; set; }
	public bool? IsActive { get; set; }

	public int SkipCount { get; set; } = 0;
	public int? MaxResultCount { get; set; }
}

