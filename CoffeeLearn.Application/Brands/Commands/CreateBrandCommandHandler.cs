using CoffeeLearn.Application.Brands.Common;
using CoffeeLearn.Application.Brands.DTOs;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Domain.Entities;
using MediatR;

namespace CoffeeLearn.Application.Brands.Commands;

public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, BrandDto>
{
	private readonly IApplicationDbContext _context;

	public CreateBrandCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<BrandDto> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
	{
		var brand = new Brand(request.Name, request.Description);

		_context.Brands.Add(brand);
		await _context.SaveChangesAsync(cancellationToken);

		return BrandMapper.ToDto(brand);
	}
}