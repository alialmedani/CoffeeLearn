using MediatR;

namespace CoffeeLearn.Application.Products.Commands;

public class DeleteProductVariantCommand : IRequest<bool>
{
	public int Id { get; set; }

	public DeleteProductVariantCommand(int id)
	{
		Id = id;
	}
}




