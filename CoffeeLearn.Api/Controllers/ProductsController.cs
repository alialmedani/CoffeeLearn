using MediatR;
using Microsoft.AspNetCore.Mvc;
using CoffeeLearn.Application.Products.Commands;
using CoffeeLearn.Application.Products.DTOs;
using CoffeeLearn.Application.Products.Queries;
using CoffeeLearn.Application.Common.Models;

namespace CoffeeLearn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
	private readonly ISender _sender;

	public ProductsController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet]
	public async Task<ActionResult<PagedResult<ProductDto>>> GetAll([FromQuery] GetProductsQuery query)
	{
		var result = await _sender.Send(query);
		return Ok(result);
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<ProductDto>> GetById(int id)
	{
		var result = await _sender.Send(new GetProductByIdQuery { Id = id });

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPost]
	public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductCommand command)
	{
		var result = await _sender.Send(command);

		return CreatedAtAction(
			nameof(GetById),
			new { id = result.Id },
			result);
	}

	[HttpPut("{id:int}")]
	public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] UpdateProductCommand command)
	{
		command.Id = id;

		var result = await _sender.Send(command);

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPut("{id:int}/activate")]
	public async Task<ActionResult<ProductDto>> Activate(int id)
	{
		var result = await _sender.Send(new ActivateProductCommand(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPut("{id:int}/deactivate")]
	public async Task<ActionResult<ProductDto>> Deactivate(int id)
	{
		var result = await _sender.Send(new DeactivateProductCommand(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var result = await _sender.Send(new DeleteProductCommand { Id = id });

		if (!result)
			return NotFound();

		return NoContent();
	}

	[HttpPut("{id:int}/restore")]
	public async Task<ActionResult<ProductDto>> Restore(int id)
	{
		var result = await _sender.Send(new RestoreProductCommand(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}
	[HttpGet("deleted")]
	public async Task<ActionResult<PagedResult<ProductDto>>> GetDeleted([FromQuery] GetDeletedProductsQuery query)
	{
		var result = await _sender.Send(query);
		return Ok(result);
	}
	 
}