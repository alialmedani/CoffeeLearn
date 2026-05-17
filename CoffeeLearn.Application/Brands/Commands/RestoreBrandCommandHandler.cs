using CoffeeLearn.Application.Brands.Common;
using CoffeeLearn.Application.Brands.DTOs;
using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Brands.Commands;

public class RestoreBrandCommandHandler : IRequestHandler<RestoreBrandCommand, BrandDto?>
{
	private readonly IApplicationDbContext _context;

	public RestoreBrandCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<BrandDto?> Handle(RestoreBrandCommand request, CancellationToken cancellationToken)
	{
		var brand = await _context.Brands
			.IgnoreQueryFilters()
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (brand is null)
			return null;

		brand.Restore();

		await _context.SaveChangesAsync(cancellationToken);

		return BrandMapper.ToDto(brand);
	}
}