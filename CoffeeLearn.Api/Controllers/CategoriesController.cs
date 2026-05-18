using CoffeeLearn.Application.Categories.Commands;
using CoffeeLearn.Application.Categories.DTOs;
using CoffeeLearn.Application.Categories.Queries;
using CoffeeLearn.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeLearn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
	private readonly ISender _sender;

	public CategoriesController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet]
	public async Task<ActionResult<PagedResult<CategoryDto>>> GetAll([FromQuery] GetCategoriesQuery query)
	{
		var result = await _sender.Send(query);
		return Ok(result);
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<CategoryDto>> GetById(int id)
	{
		var result = await _sender.Send(new GetCategoryByIdQuery { Id = id });

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPost]
	public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryCommand command)
	{
		var result = await _sender.Send(command);

		return CreatedAtAction(
			nameof(GetById),
			new { id = result.Id },
			result);
	}

	[HttpPut("{id:int}")]
	public async Task<ActionResult<CategoryDto>> Update(int id, [FromBody] UpdateCategoryCommand command)
	{
		command.Id = id;

		var result = await _sender.Send(command);

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPut("{id:int}/activate")]
	public async Task<ActionResult<CategoryDto>> Activate(int id)
	{
		var result = await _sender.Send(new ActivateCategoryCommand(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPut("{id:int}/deactivate")]
	public async Task<ActionResult<CategoryDto>> Deactivate(int id)
	{
		var result = await _sender.Send(new DeactivateCategoryCommand(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var result = await _sender.Send(new DeleteCategoryCommand { Id = id });

		if (!result)
			return NotFound();

		return NoContent();
	}

	[HttpPut("{id:int}/restore")]
	public async Task<ActionResult<CategoryDto>> Restore(int id)
	{
		var result = await _sender.Send(new RestoreCategoryCommand(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpGet("deleted")]
	public async Task<ActionResult<PagedResult<CategoryDto>>> GetDeleted([FromQuery] GetDeletedCategoriesQuery query)
	{
		var result = await _sender.Send(query);
		return Ok(result);
	}
	 

	 

	 
}