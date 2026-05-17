using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Products.Common;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Queries;

public class GetProductVariantsQueryHandler : IRequestHandler<GetProductVariantsQuery, List<ProductVariantDto>>
{
	private readonly IApplicationDbContext _context;

	public GetProductVariantsQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<ProductVariantDto>> Handle(GetProductVariantsQuery request, CancellationToken cancellationToken)
	{
		var query = _context.ProductVariants
			.AsNoTracking()
			.Include(x => x.Product)
			.Where(x => x.ProductId == request.ProductId)
			.AsQueryable();

		if (request.IsActive.HasValue)
		{
			query = query.Where(x => x.IsActive == request.IsActive.Value);
		}

		var variants = await query
			.OrderBy(x => x.Color)
			.ThenBy(x => x.Size)
			.ToListAsync(cancellationToken);

		return variants
			.Select(ProductVariantMapper.ToDto)
			.ToList();
	}
}