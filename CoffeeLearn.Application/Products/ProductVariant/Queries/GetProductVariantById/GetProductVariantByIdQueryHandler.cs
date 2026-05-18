using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Queries;

public class GetProductVariantByIdQueryHandler : IRequestHandler<GetProductVariantByIdQuery, ProductVariantDto?>
{
	private readonly IApplicationDbContext _context;

	public GetProductVariantByIdQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<ProductVariantDto?> Handle(GetProductVariantByIdQuery request, CancellationToken cancellationToken)
	{
		var variant = await _context.ProductVariants
			.AsNoTracking()
			.Include(x => x.Product)
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (variant is null)
			return null;

		return ProductVariantMapper.ToDto(variant);
	}
}






