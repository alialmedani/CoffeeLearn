using MediatR;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Queries;

public class GetProductVariantByIdQuery : IRequest<ProductVariantDto?>
{
	public int Id { get; set; }

	public GetProductVariantByIdQuery(int id)
	{
		Id = id;
	}
}