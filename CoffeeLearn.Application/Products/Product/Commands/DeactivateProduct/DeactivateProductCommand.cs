using MediatR;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Commands;

public class DeactivateProductCommand : IRequest<ProductDto?>
{
	public int Id { get; set; }

	public DeactivateProductCommand(int id)
	{
		Id = id;
	}
}