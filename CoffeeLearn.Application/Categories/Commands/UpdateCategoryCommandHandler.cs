using CoffeeLearn.Application.Categories.Common;
using CoffeeLearn.Application.Categories.DTOs;
using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Categories.Commands;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto?>
{
	private readonly IApplicationDbContext _context;

	public UpdateCategoryCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<CategoryDto?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = await _context.Categories
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (category is null)
			return null;

		category.Update(request.Name, request.Description);

		await _context.SaveChangesAsync(cancellationToken);

		return CategoryMapper.ToDto(category);
	}
}