using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Products.Queries;

public class GetProductDetailsQueryHandler : IRequestHandler<GetProductDetailsQuery, ProductDetailsDto?>
{
	private readonly IApplicationDbContext _context;

	public GetProductDetailsQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<ProductDetailsDto?> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
	{
		var product = await _context.Products
			.Include(x => x.Category)
			.Include(x => x.Brand)
			.Include(x => x.Variants)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (product is null)
			return null;

		var productDto = ProductMapper.ToDto(product);

		var variants = product.Variants
			.Where(x => !x.IsDeleted)
			.OrderBy(x => x.Color)
			.ThenBy(x => x.Size)
			.ToList();

		return new ProductDetailsDto
		{
			Id = productDto.Id,
			Name = productDto.Name,
			Price = productDto.Price,
			Description = productDto.Description,
			ImageUrl = productDto.ImageUrl,

			Category = product.Category is null
				? null
				: new LookupDto
				{
					Id = product.Category.Id,
					Name = product.Category.Name,
					Description = product.Category.Description
				},

			Brand = product.Brand is null
				? null
				: new LookupDto
				{
					Id = product.Brand.Id,
					Name = product.Brand.Name,
					Description = product.Brand.Description
				},

			TotalVariantStock = productDto.TotalVariantStock,
			ActiveVariantCount = productDto.ActiveVariantCount,
			AvailabilityStatus = productDto.AvailabilityStatus,

			Colors = variants
				.GroupBy(x => x.Color)
				.Select(group => new ProductColorGroupDto
				{
					Color = group.Key,
					ImageUrl = group.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.ImageUrl))?.ImageUrl,
					TotalStock = group.Where(x => x.IsActive).Sum(x => x.Quantity),
					Sizes = group
						.Select(x => new ProductSizeStockDto
						{
							ProductVariantId = x.Id,
							Size = x.Size,
							Quantity = x.Quantity,
							Sku = x.Sku,
							ImageUrl = x.ImageUrl,
							IsActive = x.IsActive
						})
						.ToList()
				})
				.ToList()
		};
	}
}