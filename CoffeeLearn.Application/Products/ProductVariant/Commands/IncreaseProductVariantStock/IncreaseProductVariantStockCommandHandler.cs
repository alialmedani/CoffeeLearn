using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Commands;

public class IncreaseProductVariantStockCommandHandler
	: IRequestHandler<IncreaseProductVariantStockCommand, ProductVariantDto?>
{
	private readonly IApplicationDbContext _context;

	public IncreaseProductVariantStockCommandHandler(
		IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<ProductVariantDto?> Handle(
		IncreaseProductVariantStockCommand request,
		CancellationToken cancellationToken)
	{
		var variant = await _context.ProductVariants
			.Include(x => x.Product)
			.FirstOrDefaultAsync(
				x => x.Id == request.Id,
				cancellationToken);

		if (variant is null)
			return null;

		variant.IncreaseStock(request.Quantity);

		await _context.SaveChangesAsync(cancellationToken);

		return ProductVariantMapper.ToDto(variant);
	}
}






