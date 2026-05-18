using MediatR;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Queries;

public class GetProductByIdQuery : IRequest<ProductDto?>
{
	public int Id { get; set; }
}






