using MediatR;
using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;

namespace CoffeeLearn.Application.Products.Commands;

public class DeleteProductVariantCommandHandler : IRequestHandler<DeleteProductVariantCommand, bool>
{
	private readonly IApplicationDbContext _context;

	public DeleteProductVariantCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<bool> Handle(DeleteProductVariantCommand request, CancellationToken cancellationToken)
	{
		var variant = await _context.ProductVariants
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (variant is null)
			return false;

		_context.ProductVariants.Remove(variant);

		await _context.SaveChangesAsync(cancellationToken);

		return true;
	}
}