using CoffeeLearn.Application.Brands.Common;
using CoffeeLearn.Application.Brands.DTOs;
using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Brands.Commands;

public class DeactivateBrandCommandHandler : IRequestHandler<DeactivateBrandCommand, BrandDto?>
{
	private readonly IApplicationDbContext _context;

	public DeactivateBrandCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<BrandDto?> Handle(DeactivateBrandCommand request, CancellationToken cancellationToken)
	{
		var brand = await _context.Brands
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (brand is null)
			return null;

		brand.Deactivate();

		await _context.SaveChangesAsync(cancellationToken);

		return BrandMapper.ToDto(brand);
	}
}





