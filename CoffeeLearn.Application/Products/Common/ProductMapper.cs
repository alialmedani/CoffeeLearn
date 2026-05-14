using CoffeeLearn.Application.Products.DTOs;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Products.Common;

public static class ProductMapper
{
	public static ProductDto ToDto(Product product)
	{
		return new ProductDto
		{
			Id = product.Id,
			Name = product.Name,
			Quantity = product.Quantity,
			Price = product.Price,
			CreatedAt = product.CreatedAt,
			UpdatedAt = product.UpdatedAt
		};
	}
}