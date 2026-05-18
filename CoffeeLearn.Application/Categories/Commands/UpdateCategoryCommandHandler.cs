using CoffeeLearn.Application.Categories.Common;
using CoffeeLearn.Application.Categories.DTOs;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Domain.Entities;
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
			.Include(x => x.SizeOptions)
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (category is null)
			return null;

		category.Update(request.Name, request.Description);

		foreach (var oldSizeOption in category.SizeOptions.ToList())
		{
			_context.CategorySizeOptions.Remove(oldSizeOption);
		}

		foreach (var sizeOption in request.SizeOptions)
		{
			category.SizeOptions.Add(new CategorySizeOption(
				category.Id,
				sizeOption.SizeName,
				sizeOption.SortOrder));
		}

		await _context.SaveChangesAsync(cancellationToken);

		var updatedCategory = await _context.Categories
			.AsNoTracking()
			.Include(x => x.SizeOptions)
			.FirstAsync(x => x.Id == category.Id, cancellationToken);

		return CategoryMapper.ToDto(updatedCategory);
	}
}