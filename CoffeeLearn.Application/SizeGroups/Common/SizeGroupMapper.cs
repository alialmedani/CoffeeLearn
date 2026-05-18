using CoffeeLearn.Application.SizeGroups.DTOs;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.SizeGroups.Common;

public static class SizeGroupMapper
{
	public static SizeGroupDto ToDto(SizeGroup sizeGroup)
	{
		return new SizeGroupDto
		{
			Id = sizeGroup.Id,
			CategoryId = sizeGroup.CategoryId,
			CategoryName = sizeGroup.Category?.Name ?? string.Empty,
			Name = sizeGroup.Name,
			Description = sizeGroup.Description,
			IsActive = sizeGroup.IsActive,
			CreatedAt = sizeGroup.CreatedAt,
			UpdatedAt = sizeGroup.UpdatedAt,
			SizeOptions = sizeGroup.SizeOptions
				.OrderBy(x => x.SortOrder)
				.ThenBy(x => x.Name)
				.Select(x => new SizeOptionDto
				{
					Id = x.Id,
					SizeGroupId = x.SizeGroupId,
					Name = x.Name,
					SortOrder = x.SortOrder,
					IsActive = x.IsActive,
					CreatedAt = x.CreatedAt,
					UpdatedAt = x.UpdatedAt
				})
				.ToList()
		};
	}
}


