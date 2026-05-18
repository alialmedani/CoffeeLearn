using MediatR;

namespace CoffeeLearn.Application.Categories.Commands;

public class DeleteCategoryCommand : IRequest<bool>
{
	public int Id { get; set; }
}





