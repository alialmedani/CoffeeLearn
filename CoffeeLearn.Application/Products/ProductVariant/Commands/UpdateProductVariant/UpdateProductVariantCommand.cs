using MediatR;
using CoffeeLearn.Application.Products.DTOs;

namespace CoffeeLearn.Application.Products.Commands;

public class UpdateProductVariantCommand : IRequest<ProductVariantDto?>
{
	public int Id { get; set; }

	public string Color { get; set; } = string.Empty;

	public int? SizeOptionId { get; set; }

	public int Quantity { get; set; }

	public string? Sku { get; set; }

	public string? ImageUrl { get; set; }

	public bool IsActive { get; set; }
}



