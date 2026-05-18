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

	[HttpGet("{id:int}")]
	public async Task<ActionResult<ProductVariantDto>> GetById(int id)
	{
		var result = await _sender.Send(new GetProductVariantByIdQuery(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPost]
	public async Task<ActionResult<ProductVariantDto>> Create([FromBody] CreateProductVariantCommand command)
	{
		var result = await _sender.Send(command);

		return CreatedAtAction(
			nameof(GetById),
			new { id = result.Id },
			result);
	}
	[HttpPut("{id:int}")]
	public async Task<ActionResult<ProductVariantDto>> Update(int id, [FromBody] UpdateProductVariantCommand command)
	{
		command.Id = id;

		var result = await _sender.Send(command);

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var result = await _sender.Send(new DeleteProductVariantCommand(id));

		if (!result)
			return NotFound();

		return NoContent();
	}
}