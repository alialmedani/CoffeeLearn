using CoffeeLearn.Application.Categories.Common;
using CoffeeLearn.Application.Categories.DTOs;
using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Categories.Commands;

public class RestoreCategoryCommandHandler : IRequestHandler<RestoreCategoryCommand, CategoryDto?>
{
	private readonly IApplicationDbContext _context;

	public RestoreCategoryCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<CategoryDto?> Handle(RestoreCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = await _context.Categories
			.IgnoreQueryFilters()
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (category is null)
			return null;

		category.Restore();

		await _context.SaveChangesAsync(cancellationToken);

		return CategoryMapper.ToDto(category);
	}
}





