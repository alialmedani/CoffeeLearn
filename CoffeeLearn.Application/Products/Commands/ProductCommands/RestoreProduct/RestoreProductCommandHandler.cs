using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Commands.Product.RestoreProduct;

public class RestoreProductCommandHandler : IRequestHandler<RestoreProductCommand, ProductDto?>
{
	private readonly IApplicationDbContext _context;

	public RestoreProductCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<ProductDto?> Handle(RestoreProductCommand request, CancellationToken cancellationToken)
	{
		var product = await _context.Products
			.IgnoreQueryFilters()
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (product is null)
			return null;

		product.Restore();

		await _context.SaveChangesAsync(cancellationToken);

		return ProductMapper.ToDto(product);
	}
}