using MediatR;
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
		var product = new Product
		{
			Name = request.Name,
			Quantity = request.Quantity,
			Price = request.Price,
			Description = request.Description,
			ImageUrl = request.ImageUrl,
		};

		_context.Products.Add(product);
		await _context.SaveChangesAsync(cancellationToken);

		return ProductMapper.ToDto(product);
	}
}