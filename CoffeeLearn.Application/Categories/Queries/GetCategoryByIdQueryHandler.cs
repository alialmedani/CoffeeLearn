using CoffeeLearn.Application.Categories.Common;
using CoffeeLearn.Application.Categories.DTOs;
using CoffeeLearn.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.Categories.Queries;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
	private readonly IApplicationDbContext _context;

	public GetCategoryByIdQueryHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
	{
		var category = await _context.Categories
			.AsNoTracking()
 			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (category is null)
			return null;

		return CategoryMapper.ToDto(category);
	}
}




