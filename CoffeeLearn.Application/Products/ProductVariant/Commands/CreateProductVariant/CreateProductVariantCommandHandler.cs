using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Exceptions;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Commands;

public class CreateProductVariantCommandHandler : IRequestHandler<CreateProductVariantCommand, ProductVariantDto>
{
	private readonly IApplicationDbContext _context;

	public CreateProductVariantCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<ProductVariantDto> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
	{
		var product = await _context.Products
			.Include(x => x.Category)
			.FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

		if (product is null)
			throw new NotFoundException("Product does not exist.");

		if (!product.CategoryId.HasValue || product.Category is null)
			throw new BusinessRuleException("Product must have a category before adding variants.");

		var sizeOption = await _context.SizeOptions
			.Include(x => x.SizeGroup)
			.FirstOrDefaultAsync(x => x.Id == request.SizeOptionId!.Value, cancellationToken);

		if (sizeOption is null)
			throw new BusinessRuleException("Size option not found.");

		if (!sizeOption.IsActive)
			throw new BusinessRuleException("Size option is inactive.");

		if (sizeOption.SizeGroup.CategoryId != product.CategoryId)
			throw new BusinessRuleException("Size option does not belong to the product category.");

		var requestedSizeOptionId = request.SizeOptionId!.Value;
		var requestedColor = request.Color.Trim();

		var duplicateVariantExists = await _context.ProductVariants
			.AnyAsync(x =>
				x.ProductId == request.ProductId &&
				x.Color.ToLower() == requestedColor.ToLower() &&
				x.SizeOptionId == requestedSizeOptionId,
				cancellationToken);

		if (duplicateVariantExists)
			throw new BusinessRuleException("Duplicate color/size variant is not allowed for the same product.");

		var variant = new CoffeeLearn.Domain.Entities.ProductVariant
		{
			ProductId = request.ProductId,
			Color = requestedColor,
			SizeOptionId = requestedSizeOptionId,
			Quantity = request.Quantity,
			Sku = string.IsNullOrWhiteSpace(request.Sku) ? null : request.Sku.Trim(),
			ImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl.Trim(),
			IsActive = request.IsActive
		};

		_context.ProductVariants.Add(variant);

		await _context.SaveChangesAsync(cancellationToken);

		var createdVariant = await _context.ProductVariants
			.AsNoTracking()
			.Include(x => x.Product)
			.Include(x => x.SizeOption)
			.FirstAsync(x => x.Id == variant.Id, cancellationToken);

		return ProductVariantMapper.ToDto(createdVariant);
	}
}


