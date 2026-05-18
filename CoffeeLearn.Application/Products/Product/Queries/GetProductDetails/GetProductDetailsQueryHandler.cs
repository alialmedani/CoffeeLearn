using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;
using CoffeeLearn.Application.Interfaces;
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

		var activeVariants = product.Variants
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

			CategoryId = productDto.CategoryId,
			CategoryName = productDto.CategoryName,

			BrandId = productDto.BrandId,
			BrandName = productDto.BrandName,

			TotalVariantStock = productDto.TotalVariantStock,
			ActiveVariantCount = productDto.ActiveVariantCount,
			AvailabilityStatus = productDto.AvailabilityStatus,

			Colors = activeVariants
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