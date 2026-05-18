using CoffeeLearn.Application.Brands.DTOs;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Brands.Common;

public static class BrandMapper
{
	public static BrandDto ToDto(Brand brand)
	{
		return new BrandDto
		{
			Id = brand.Id,
			Name = brand.Name,
			Description = brand.Description,
			IsActive = brand.IsActive,
			IsDeleted = brand.IsDeleted,
			CreatedAt = brand.CreatedAt,
			UpdatedAt = brand.UpdatedAt,
			DeletedAt = brand.DeletedAt
		};
	}
}





