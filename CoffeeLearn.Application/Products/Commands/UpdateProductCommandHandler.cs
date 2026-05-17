using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Products.Commands;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto?>
{
	private readonly IApplicationDbContext _context;

	public UpdateProductCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<ProductDto?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
	{
		var product = await _context.Products
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (product is null)
			return null;

		product.Name = request.Name;
		product.Quantity = request.Quantity;
		product.Price = request.Price;
		product.Description = request.Description;
		product.ImageUrl = request.ImageUrl;

		await _context.SaveChangesAsync(cancellationToken);

		return ProductMapper.ToDto(product);
	}
}