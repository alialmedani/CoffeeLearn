using MediatR;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Commands;

public class RestoreProductCommand : IRequest<ProductDto?>
{
	public int Id { get; set; }

	public RestoreProductCommand(int id)
	{
		Id = id;
	}
}