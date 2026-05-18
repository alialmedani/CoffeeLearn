using CoffeeLearn.Application.Products.DTOs;
using MediatR;

namespace CoffeeLearn.Application.Products.Queries;

public class GetProductDetailsQuery : IRequest<ProductDetailsDto?>
{
	public int Id { get; set; }
}






