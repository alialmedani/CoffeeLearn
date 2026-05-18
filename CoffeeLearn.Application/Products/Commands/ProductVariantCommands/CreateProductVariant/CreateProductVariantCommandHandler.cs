using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Common.Exceptions;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Products.Commands.ProductVariant.CreateProductVariant;

public class CreateProductVariantCommandHandler : IRequestHandler<CreateProductVariantCommand, ProductVariantDto>
{
	private readonly IApplicationDbContext _context;

	public CreateProductVariantCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<ProductVariantDto> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
	{
		var productExists = await _context.Products
			.AnyAsync(x => x.Id == request.ProductId, cancellationToken);

		if (!productExists)
			throw new NotFoundException("Product does not exist.");

		var variant = new CoffeeLearn.Domain.Entities.ProductVariant
		{
			ProductId = request.ProductId,
			Color = request.Color.Trim(),
			Size = request.Size.Trim(),
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