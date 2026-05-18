using CoffeeLearn.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Products.Common;

public static class ProductQueryableExtensions
{
	public static IQueryable<Product> IncludeProductDetails(this IQueryable<Product> query)
	{
		return query
			.IncludeProductDetails();
	}
}

