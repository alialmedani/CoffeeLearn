using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Brands.Commands;

public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, bool>
{
	private readonly IApplicationDbContext _context;

	public DeleteBrandCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<bool> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
	{
		var brand = await _context.Brands
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (brand is null)
			return false;

		_context.Brands.Remove(brand);
		await _context.SaveChangesAsync(cancellationToken);

		return true;
	}
}





