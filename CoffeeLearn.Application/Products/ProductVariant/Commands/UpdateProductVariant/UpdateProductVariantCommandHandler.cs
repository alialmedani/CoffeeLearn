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
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (variant is null)
			return null;

		if (!variant.Product.CategoryId.HasValue || variant.Product.Category is null)
			throw new BusinessRuleException("Product must have a category before updating variants.");

		var sizeOption = await _context.SizeOptions
			.Include(x => x.SizeGroup)
			.FirstOrDefaultAsync(x => x.Id == request.SizeOptionId!.Value, cancellationToken);

		if (sizeOption is null)
			throw new BusinessRuleException("Size option not found.");

		if (!sizeOption.IsActive)
			throw new BusinessRuleException("Size option is inactive.");

		if (sizeOption.SizeGroup.CategoryId != variant.Product.CategoryId)
			throw new BusinessRuleException("Size option does not belong to the product category.");

		var requestedSizeOptionId = request.SizeOptionId!.Value;
		var requestedColor = request.Color.Trim();

		var duplicateVariantExists = await _context.ProductVariants
			.AnyAsync(x =>
				x.Id != request.Id &&
				x.ProductId == variant.ProductId &&
				x.Color.ToLower() == requestedColor.ToLower() &&
				x.SizeOptionId == requestedSizeOptionId,
				cancellationToken);

		if (duplicateVariantExists)
			throw new BusinessRuleException("Duplicate color/size variant is not allowed for the same product.");

		variant.Color = requestedColor;
		variant.SizeOptionId = requestedSizeOptionId;
		variant.Quantity = request.Quantity;
		variant.Sku = string.IsNullOrWhiteSpace(request.Sku) ? null : request.Sku.Trim();
		variant.ImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl.Trim();
		variant.IsActive = request.IsActive;

		await _context.SaveChangesAsync(cancellationToken);

		var updatedVariant = await _context.ProductVariants
			.AsNoTracking()
			.Include(x => x.Product)
			.Include(x => x.SizeOption)
			.FirstAsync(x => x.Id == variant.Id, cancellationToken);

		return ProductVariantMapper.ToDto(updatedVariant);
	}
}
