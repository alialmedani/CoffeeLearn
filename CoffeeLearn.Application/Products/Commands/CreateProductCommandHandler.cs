using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Products.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
	private readonly IApplicationDbContext _context;

	public CreateProductCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
	{
		if (request.CategoryId.HasValue)
		{
			var categoryExists = await _context.Categories
				.AnyAsync(x => x.Id == request.CategoryId.Value, cancellationToken);

			if (!categoryExists)
				throw new InvalidOperationException("Category not found.");
		}

		var product = new Product
		{
			Name = request.Name,
			Quantity = request.Quantity,
			Price = request.Price,
			Description = request.Description,
			ImageUrl = request.ImageUrl,
			CategoryId = request.CategoryId
		};

		_context.Products.Add(product);
		await _context.SaveChangesAsync(cancellationToken);

		if (product.CategoryId.HasValue)
		{
			await _context.Products
				.Include(x => x.Category)
				.FirstOrDefaultAsync(x => x.Id == product.Id, cancellationToken);
		}

		return ProductMapper.ToDto(product);
	}
}