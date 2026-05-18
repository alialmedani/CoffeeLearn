using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Categories.Commands;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
	private readonly IApplicationDbContext _context;

	public DeleteCategoryCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = await _context.Categories
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (category is null)
			return false;

		_context.Categories.Remove(category);
		await _context.SaveChangesAsync(cancellationToken);

		return true;
	}
}




