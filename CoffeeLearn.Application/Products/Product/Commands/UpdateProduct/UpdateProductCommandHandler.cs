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
			.Include(x => x.Category)
			.Include(x => x.Brand)
			.Include(x => x.Variants)
				.ThenInclude(x => x.SizeOption)
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (product is null)
			return null;

		if (request.CategoryId.HasValue)
		{
			var categoryExists = await _context.Categories
				.AnyAsync(x => x.Id == request.CategoryId.Value, cancellationToken);

			if (!categoryExists)
				throw new InvalidOperationException("Category not found.");
		}

		if (request.BrandId.HasValue)
		{
			var brandExists = await _context.Brands
				.AnyAsync(x => x.Id == request.BrandId.Value, cancellationToken);

			if (!brandExists)
				throw new InvalidOperationException("Brand not found.");
		}

		product.Name = request.Name;
		product.Price = request.Price;
		product.Description = request.Description;
		product.ImageUrl = request.ImageUrl;
		product.CategoryId = request.CategoryId;
		product.BrandId = request.BrandId;

		await _context.SaveChangesAsync(cancellationToken);

		return ProductMapper.ToDto(product);
	}
}