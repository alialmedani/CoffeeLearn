using MediatR;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Commands;

public class ActivateProductCommand : IRequest<ProductDto?>
{
	public int Id { get; set; }

	public ActivateProductCommand(int id)
	{
		Id = id;
	}
}






