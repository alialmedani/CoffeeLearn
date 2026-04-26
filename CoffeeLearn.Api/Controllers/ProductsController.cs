using MediatR;
using Microsoft.AspNetCore.Mvc;
using CoffeeLearn.Application.Products.Commands;
using CoffeeLearn.Application.Products.DTOs;
using CoffeeLearn.Application.Products.Queries;

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
	public async Task<ActionResult<List<ProductDto>>> GetAll()
	{
		var result = await _sender.Send(new GetProductsQuery());
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
		return Ok(result);
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

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var result = await _sender.Send(new DeleteProductCommand { Id = id });

		if (!result)
			return NotFound();

		return NoContent();
	}
}