using MediatR;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Queries;

public class GetProductVariantsQuery : IRequest<List<ProductVariantDto>>
{
	public int ProductId { get; set; }

	public bool? IsActive { get; set; }
}