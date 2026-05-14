using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Queries;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
	private readonly IApplicationDbContext _context;

	public GetProductByIdQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
	{
		return await _context.Products
			.AsNoTracking()
			.Where(x => x.Id == request.Id)
			.Select(x => new ProductDto
			{
				Id = x.Id,
				Name = x.Name,
				Quantity = x.Quantity,
				Price = x.Price,
				CreatedAt = x.CreatedAt,
				UpdatedAt = x.UpdatedAt
			})
			.FirstOrDefaultAsync(cancellationToken);
	}
}