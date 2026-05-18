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
			.Include(x => x.Category!.SizeOptions)
			.FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

		if (product is null)
			throw new NotFoundException("Product does not exist.");

		if (!product.CategoryId.HasValue || product.Category is null)
			throw new BusinessRuleException("Product must have a category before adding variants.");

		var activeSizeOptions = product.Category.SizeOptions
			.Where(x => x.IsActive)
			.Select(x => x.SizeName.Trim().ToLower())
			.ToHashSet();

		if (!activeSizeOptions.Any())
			throw new BusinessRuleException("Product category does not have active size options.");

		var requestedSize = request.Size.Trim();

		if (!activeSizeOptions.Contains(requestedSize.ToLower()))
			throw new BusinessRuleException($"Invalid size option for this product category: {requestedSize}.");

		var duplicateVariantExists = await _context.ProductVariants
			.AnyAsync(x =>
				x.ProductId == request.ProductId &&
				x.Color.ToLower() == request.Color.Trim().ToLower() &&
				x.Size.ToLower() == requestedSize.ToLower(),
				cancellationToken);

		if (duplicateVariantExists)
			throw new BusinessRuleException("Duplicate color/size variant is not allowed for the same product.");

		var variant = new CoffeeLearn.Domain.Entities.ProductVariant
		{
			ProductId = request.ProductId,
			Color = request.Color.Trim(),
			Size = requestedSize,
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
			.FirstAsync(x => x.Id == variant.Id, cancellationToken);

		return ProductVariantMapper.ToDto(createdVariant);
	}
}