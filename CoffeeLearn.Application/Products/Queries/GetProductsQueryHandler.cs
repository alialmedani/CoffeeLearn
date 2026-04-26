using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Queries;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
	private readonly IApplicationDbContext _context;

	public GetProductsQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
	{
		return await _context.Products
			.AsNoTracking()
			.Select(x => new ProductDto
			{
				Id = x.Id,
				Name = x.Name,
				Quantity = x.Quantity,
				Price = x.Price
			})
			.ToListAsync(cancellationToken);
	}
}