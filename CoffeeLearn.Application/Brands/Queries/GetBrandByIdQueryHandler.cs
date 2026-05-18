using CoffeeLearn.Application.Brands.Common;
using CoffeeLearn.Application.Brands.DTOs;
using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Brands.Queries;

public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, BrandDto?>
{
	private readonly IApplicationDbContext _context;

	public GetBrandByIdQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<BrandDto?> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
	{
		var brand = await _context.Brands
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (brand is null)
			return null;

		return BrandMapper.ToDto(brand);
	}
}




