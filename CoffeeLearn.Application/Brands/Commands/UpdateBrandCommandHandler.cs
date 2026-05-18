using CoffeeLearn.Application.Brands.Common;
using CoffeeLearn.Application.Brands.DTOs;
using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Brands.Commands;

public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, BrandDto?>
{
	private readonly IApplicationDbContext _context;

	public UpdateBrandCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<BrandDto?> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
	{
		var brand = await _context.Brands
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (brand is null)
			return null;

		brand.Update(request.Name, request.Description);

		await _context.SaveChangesAsync(cancellationToken);

		return BrandMapper.ToDto(brand);
	}
}




