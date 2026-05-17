using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.Reports.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Reports.Queries;

public class GetLowStockVariantsQueryHandler : IRequestHandler<GetLowStockVariantsQuery, List<LowStockVariantDto>>
{
	private readonly IApplicationDbContext _context;

	public GetLowStockVariantsQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<LowStockVariantDto>> Handle(GetLowStockVariantsQuery request, CancellationToken cancellationToken)
	{
		var threshold = request.Threshold <= 0 ? 5 : request.Threshold;

		var query = _context.ProductVariants
			.Include(x => x.Product)
				.ThenInclude(x => x.Category)
			.Include(x => x.Product)
				.ThenInclude(x => x.Brand)
			.AsNoTracking()
			.Where(x => x.Quantity <= threshold)
			.AsQueryable();

		if (request.OnlyActive)
		{
			query = query.Where(x => x.IsActive && x.Product.IsActive);
		}

		return await query
			.OrderBy(x => x.Quantity)
			.ThenBy(x => x.Product.Name)
			.Select(x => new LowStockVariantDto
			{
				ProductId = x.ProductId,
				ProductName = x.Product.Name,

				CategoryId = x.Product.CategoryId,
				CategoryName = x.Product.Category != null ? x.Product.Category.Name : null,

				BrandId = x.Product.BrandId,
				BrandName = x.Product.Brand != null ? x.Product.Brand.Name : null,

				ProductVariantId = x.Id,
				Color = x.Color,
				Size = x.Size,
				Sku = x.Sku,

				Quantity = x.Quantity,
				IsActive = x.IsActive
			})
			.ToListAsync(cancellationToken);
	}
}