using CoffeeLearn.Application.Categories.Common;
using CoffeeLearn.Application.Categories.DTOs;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Categories.Commands;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
	private readonly IApplicationDbContext _context;

	public CreateCategoryCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = new Category(request.Name, request.Description);

		foreach (var sizeOption in request.SizeOptions)
		{
			category.SizeOptions.Add(new CategorySizeOption(
				category.Id,
				sizeOption.SizeName,
				sizeOption.SortOrder));
		}

		_context.Categories.Add(category);
		await _context.SaveChangesAsync(cancellationToken);

		var createdCategory = await _context.Categories
			.AsNoTracking()
			.Include(x => x.SizeOptions)
			.FirstAsync(x => x.Id == category.Id, cancellationToken);

		return CategoryMapper.ToDto(createdCategory);
	}
}