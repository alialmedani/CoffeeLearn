using CoffeeLearn.Application.Categories.DTOs;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Categories.Common;

public static class CategoryMapper
{
	public static CategoryDto ToDto(Category category)
	{
		return new CategoryDto
		{
			Id = category.Id,
			Name = category.Name,
			Description = category.Description,
			IsActive = category.IsActive,
			IsDeleted = category.IsDeleted,
			CreatedAt = category.CreatedAt,
			UpdatedAt = category.UpdatedAt,
			DeletedAt = category.DeletedAt,
			SizeOptions = category.SizeOptions
				.OrderBy(x => x.SortOrder)
				.ThenBy(x => x.SizeName)
				.Select(x => new CategorySizeOptionDto
				{
					Id = x.Id,
					CategoryId = x.CategoryId,
					SizeName = x.SizeName,
					SortOrder = x.SortOrder,
					IsActive = x.IsActive,
					CreatedAt = x.CreatedAt,
					UpdatedAt = x.UpdatedAt
				})
				.ToList()
		};
	}
}