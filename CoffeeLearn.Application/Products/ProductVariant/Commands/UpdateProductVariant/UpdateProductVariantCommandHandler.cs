using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Exceptions;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Commands;

public class UpdateProductVariantCommandHandler : IRequestHandler<UpdateProductVariantCommand, ProductVariantDto?>
{
	private readonly IApplicationDbContext _context;

	public UpdateProductVariantCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<ProductVariantDto?> Handle(UpdateProductVariantCommand request, CancellationToken cancellationToken)
	{
		var variant = await _context.ProductVariants
			.Include(x => x.Product)
				.ThenInclude(x => x.Category)
					.ThenInclude(x => x!.SizeOptions)
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (variant is null)
			return null;

		if (!variant.Product.CategoryId.HasValue || variant.Product.Category is null)
			throw new BusinessRuleException("Product must have a category before updating variants.");

		var activeSizeOptions = variant.Product.Category.SizeOptions
			.Where(x => x.IsActive)
			.Select(x => x.SizeName.Trim().ToLower())
			.ToHashSet();

		if (!activeSizeOptions.Any())
			throw new BusinessRuleException("Product category does not have active size options.");

		var requestedSize = request.Size.Trim();

		if (!activeSizeOptions.Contains(requestedSize.ToLower()))
			throw new BusinessRuleException($"Invalid size option for this product category: {requestedSize}.");

		var requestedColor = request.Color.Trim();

		var duplicateVariantExists = await _context.ProductVariants
			.AnyAsync(x =>
				x.Id != request.Id &&
				x.ProductId == variant.ProductId &&
				x.Color.ToLower() == requestedColor.ToLower() &&
				x.Size.ToLower() == requestedSize.ToLower(),
				cancellationToken);

		if (duplicateVariantExists)
			throw new BusinessRuleException("Duplicate color/size variant is not allowed for the same product.");

		variant.Color = requestedColor;
		variant.Size = requestedSize;
		variant.Quantity = request.Quantity;
		variant.Sku = string.IsNullOrWhiteSpace(request.Sku) ? null : request.Sku.Trim();
		variant.ImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl.Trim();
		variant.IsActive = request.IsActive;

		await _context.SaveChangesAsync(cancellationToken);

		return ProductVariantMapper.ToDto(variant);
	}
}