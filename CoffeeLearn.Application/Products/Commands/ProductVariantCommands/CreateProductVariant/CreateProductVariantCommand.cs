using MediatR;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Commands;

public class CreateProductVariantCommand : IRequest<ProductVariantDto>
{
	public int ProductId { get; set; }

	public string Color { get; set; } = string.Empty;

	public string Size { get; set; } = string.Empty;

	public int Quantity { get; set; }

	public string? Sku { get; set; }

	public string? ImageUrl { get; set; }

	public bool IsActive { get; set; } = true;
}