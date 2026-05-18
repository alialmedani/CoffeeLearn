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
			DeletedAt = category.DeletedAt
		};
	}
}

