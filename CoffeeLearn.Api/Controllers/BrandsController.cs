using CoffeeLearn.Application.Brands.Commands;
using CoffeeLearn.Application.Brands.DTOs;
using CoffeeLearn.Application.Brands.Queries;
using CoffeeLearn.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeLearn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandsController : ControllerBase
{
	private readonly ISender _sender;

	public BrandsController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet]
	public async Task<ActionResult<PagedResult<BrandDto>>> GetAll([FromQuery] GetBrandsQuery query)
	{
		var result = await _sender.Send(query);
		return Ok(result);
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<BrandDto>> GetById(int id)
	{
		var result = await _sender.Send(new GetBrandByIdQuery { Id = id });

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPost]
	public async Task<ActionResult<BrandDto>> Create([FromBody] CreateBrandCommand command)
	{
		var result = await _sender.Send(command);

		return CreatedAtAction(
			nameof(GetById),
			new { id = result.Id },
			result);
	}

	[HttpPut("{id:int}")]
	public async Task<ActionResult<BrandDto>> Update(int id, [FromBody] UpdateBrandCommand command)
	{
		command.Id = id;

		var result = await _sender.Send(command);

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPut("{id:int}/activate")]
	public async Task<ActionResult<BrandDto>> Activate(int id)
	{
		var result = await _sender.Send(new ActivateBrandCommand(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPut("{id:int}/deactivate")]
	public async Task<ActionResult<BrandDto>> Deactivate(int id)
	{
		var result = await _sender.Send(new DeactivateBrandCommand(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var result = await _sender.Send(new DeleteBrandCommand { Id = id });

		if (!result)
			return NotFound();

		return NoContent();
	}

	[HttpPut("{id:int}/restore")]
	public async Task<ActionResult<BrandDto>> Restore(int id)
	{
		var result = await _sender.Send(new RestoreBrandCommand(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpGet("deleted")]
	public async Task<ActionResult<PagedResult<BrandDto>>> GetDeleted([FromQuery] GetDeletedBrandsQuery query)
	{
		var result = await _sender.Send(query);
		return Ok(result);
	}
}