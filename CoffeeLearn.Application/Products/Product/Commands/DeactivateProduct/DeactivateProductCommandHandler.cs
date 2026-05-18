using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Commands;

public class DeactivateProductCommandHandler : IRequestHandler<DeactivateProductCommand, ProductDto?>
{
	private readonly IApplicationDbContext _context;

	public DeactivateProductCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<ProductDto?> Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
	{
		var product = await _context.Products
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (product is null)
			return null;

		product.Deactivate();

		await _context.SaveChangesAsync(cancellationToken);

		return ProductMapper.ToDto(product);
	}
}