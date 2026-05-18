using CoffeeLearn.Application.Common.Exceptions;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Application.SizeGroups.Common;
using CoffeeLearn.Application.SizeGroups.DTOs;
using CoffeeLearn.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Application.SizeGroups.Commands;

public class CreateSizeGroupCommandHandler : IRequestHandler<CreateSizeGroupCommand, SizeGroupDto>
{
	private readonly IApplicationDbContext _context;

	public CreateSizeGroupCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<SizeGroupDto> Handle(CreateSizeGroupCommand request, CancellationToken cancellationToken)
	{
		var category = await _context.Categories
			.FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);

		if (category is null)
			throw new BusinessRuleException("Category not found.");

		if (!category.IsActive)
			throw new BusinessRuleException("Category is inactive.");

		var duplicateGroupExists = await _context.SizeGroups
			.AnyAsync(x =>
				x.CategoryId == request.CategoryId &&
				x.Name.ToLower() == request.Name.Trim().ToLower(),
				cancellationToken);

		if (duplicateGroupExists)
			throw new BusinessRuleException("Size group already exists for this category.");

		var duplicateSizes = request.SizeOptions
			.GroupBy(x => x.Name.Trim().ToLower())
			.Any(x => x.Count() > 1);

		if (duplicateSizes)
			throw new BusinessRuleException("Duplicate size options are not allowed.");

		var sizeGroup = new SizeGroup(
			request.CategoryId,
			request.Name.Trim(),
			string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim());

		foreach (var option in request.SizeOptions)
		{
			sizeGroup.SizeOptions.Add(new SizeOption(
				sizeGroup.Id,
				option.Name.Trim(),
				option.SortOrder));
		}

		_context.SizeGroups.Add(sizeGroup);

		await _context.SaveChangesAsync(cancellationToken);

		var createdSizeGroup = await _context.SizeGroups
			.AsNoTracking()
			.Include(x => x.Category)
			.Include(x => x.SizeOptions)
			.FirstAsync(x => x.Id == sizeGroup.Id, cancellationToken);

		return SizeGroupMapper.ToDto(createdSizeGroup);
	}
}

