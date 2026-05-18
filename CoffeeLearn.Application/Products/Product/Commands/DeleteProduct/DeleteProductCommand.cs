using MediatR;

namespace CoffeeLearn.Application.Products.Commands;

public class DeleteProductCommand : IRequest<bool>
{
	public int Id { get; set; }
}