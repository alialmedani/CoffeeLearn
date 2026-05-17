using MediatR;
using Microsoft.EntityFrameworkCore;
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
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (variant is null)
			return null;

		variant.Color = request.Color.Trim();
		variant.Size = request.Size.Trim();
		variant.Quantity = request.Quantity;
		variant.Sku = string.IsNullOrWhiteSpace(request.Sku) ? null : request.Sku.Trim();
		variant.IsActive = request.IsActive;

		await _context.SaveChangesAsync(cancellationToken);

		return ProductVariantMapper.ToDto(variant);
	}
}