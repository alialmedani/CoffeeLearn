using MediatR;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Commands;

public class IncreaseProductVariantStockCommand : IRequest<ProductVariantDto?>
{
	public int Id { get; set; }

	public int Quantity { get; set; }
}






