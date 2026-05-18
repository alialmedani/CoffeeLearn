using CoffeeLearn.Application.Brands.Common;
using CoffeeLearn.Application.Brands.DTOs;
using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Brands.Commands;

public class ActivateBrandCommandHandler : IRequestHandler<ActivateBrandCommand, BrandDto?>
{
	private readonly IApplicationDbContext _context;

	public ActivateBrandCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<BrandDto?> Handle(ActivateBrandCommand request, CancellationToken cancellationToken)
	{
		var brand = await _context.Brands
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (brand is null)
			return null;

		brand.Activate();

		await _context.SaveChangesAsync(cancellationToken);

		return BrandMapper.ToDto(brand);
	}
}




