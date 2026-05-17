using CoffeeLearn.Application.Categories.Common;
using CoffeeLearn.Application.Categories.DTOs;
using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Categories.Commands;

public class ActivateCategoryCommandHandler : IRequestHandler<ActivateCategoryCommand, CategoryDto?>
{
	private readonly IApplicationDbContext _context;

	public ActivateCategoryCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<CategoryDto?> Handle(ActivateCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = await _context.Categories
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (category is null)
			return null;

		category.Activate();

		await _context.SaveChangesAsync(cancellationToken);

		return CategoryMapper.ToDto(category);
	}
}