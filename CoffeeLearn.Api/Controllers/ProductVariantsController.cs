using MediatR;
using Microsoft.AspNetCore.Mvc;
using CoffeeLearn.Application.Products.Commands;
using CoffeeLearn.Application.Products.DTOs;
using CoffeeLearn.Application.Products.Queries;

namespace CoffeeLearn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductVariantsController : ControllerBase
{
	private readonly ISender _sender;

	public ProductVariantsController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet]
	public async Task<ActionResult<List<ProductVariantDto>>> GetAll([FromQuery] GetProductVariantsQuery query)
	{
		var result = await _sender.Send(query);
		return Ok(result);
	}

	[HttpPost]
	public async Task<ActionResult<ProductVariantDto>> Create([FromBody] CreateProductVariantCommand command)
	{
		var result = await _sender.Send(command);

		return CreatedAtAction(
			nameof(GetAll),
			new { productId = result.ProductId },
			result);
	}
}