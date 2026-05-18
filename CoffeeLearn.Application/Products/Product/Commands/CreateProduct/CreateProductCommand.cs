using MediatR;
using CoffeeLearn.Application.Products.DTOs;
namespace CoffeeLearn.Application.Products.Commands;


public class CreateProductCommand : IRequest<ProductDto>
{
	public string Name { get; set; } = string.Empty;
	public int Quantity { get; set; }
	public decimal Price { get; set; }
	public string? Description { get; set; }
	public string? ImageUrl { get; set; }

	public int? CategoryId { get; set; }
	public int? BrandId { get; set; }
}




