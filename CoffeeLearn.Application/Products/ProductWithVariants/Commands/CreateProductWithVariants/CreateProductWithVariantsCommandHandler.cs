using CoffeeLearn.Application.Common.Exceptions;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Products.Commands;

public class CreateProductWithVariantsCommandHandler : IRequestHandler<CreateProductWithVariantsCommand, ProductDto>
{
	private readonly IApplicationDbContext _context;

	public CreateProductWithVariantsCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<ProductDto> Handle(CreateProductWithVariantsCommand request, CancellationToken cancellationToken)
	{
		if (!request.CategoryId.HasValue)
			throw new BusinessRuleException("Category is required when creating product with variants.");

		var category = await _context.Categories
			.Include(x => x.SizeOptions)
			.FirstOrDefaultAsync(x => x.Id == request.CategoryId.Value, cancellationToken);

		if (category is null)
			throw new BusinessRuleException("Category not found.");

		if (!category.IsActive)
			throw new BusinessRuleException("Category is inactive.");

		var activeSizeOptions = category.SizeOptions
			.Where(x => x.IsActive)
			.Select(x => x.SizeName.Trim().ToLower())
			.ToHashSet();

		if (!activeSizeOptions.Any())
			throw new BusinessRuleException("Category does not have active size options.");

		var invalidSizes = request.Variants
			.Select(x => x.Size.Trim())
			.Where(size => !activeSizeOptions.Contains(size.ToLower()))
			.Distinct()
			.ToList();

		if (invalidSizes.Any())
		{
			throw new BusinessRuleException(
				$"Invalid size option(s) for this category: {string.Join(", ", invalidSizes)}.");
		}

		if (request.BrandId.HasValue)
		{
			var brandExists = await _context.Brands
				.AnyAsync(x => x.Id == request.BrandId.Value, cancellationToken);

			if (!brandExists)
				throw new BusinessRuleException("Brand not found.");
		}

		var duplicateVariants = request.Variants
			.GroupBy(x => new
			{
				Color = x.Color.Trim().ToLower(),
				Size = x.Size.Trim().ToLower()
			})
			.Any(x => x.Count() > 1);

		if (duplicateVariants)
			throw new BusinessRuleException("Duplicate color/size variants are not allowed for the same product.");

		var product = new Domain.Entities.Product
		{
			Name = request.Name.Trim(),
			Quantity = 0,
			Price = request.Price,
			Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
			ImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl.Trim(),
			CategoryId = request.CategoryId,
			BrandId = request.BrandId
		};

		foreach (var item in request.Variants)
		{
			product.Variants.Add(new Domain.Entities.ProductVariant
			{
				Color = item.Color.Trim(),
				Size = item.Size.Trim(),
				Quantity = item.Quantity,
				Sku = string.IsNullOrWhiteSpace(item.Sku) ? null : item.Sku.Trim(),
				ImageUrl = string.IsNullOrWhiteSpace(item.ImageUrl) ? null : item.ImageUrl.Trim(),
				IsActive = item.IsActive
			});
		}

		_context.Products.Add(product);

		await _context.SaveChangesAsync(cancellationToken);

		var createdProduct = await _context.Products
			.Include(x => x.Category)
			.Include(x => x.Brand)
			.Include(x => x.Variants)
			.AsNoTracking()
			.FirstAsync(x => x.Id == product.Id, cancellationToken);

		return ProductMapper.ToDto(createdProduct);
	}
}