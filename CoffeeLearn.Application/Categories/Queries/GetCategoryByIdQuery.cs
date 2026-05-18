using CoffeeLearn.Application.Categories.DTOs;
using MediatR;

namespace CoffeeLearn.Application.Categories.Queries;

public class GetCategoryByIdQuery : IRequest<CategoryDto?>
{
	public int Id { get; set; }
}





