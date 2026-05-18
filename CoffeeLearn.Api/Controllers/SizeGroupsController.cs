using CoffeeLearn.Application.Common.Models;
using CoffeeLearn.Application.SizeGroups.Commands;
using CoffeeLearn.Application.SizeGroups.DTOs;
using CoffeeLearn.Application.SizeGroups.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeLearn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SizeGroupsController : ControllerBase
{
	private readonly ISender _sender;

	public SizeGroupsController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet]
	public async Task<ActionResult<PagedResult<SizeGroupDto>>> GetAll([FromQuery] GetSizeGroupsQuery query)
	{
		var result = await _sender.Send(query);
		return Ok(result);
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<SizeGroupDto>> GetById(int id)
	{
		var result = await _sender.Send(new GetSizeGroupByIdQuery { Id = id });

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPost]
	public async Task<ActionResult<SizeGroupDto>> Create([FromBody] CreateSizeGroupCommand command)
	{
		var result = await _sender.Send(command);

		return CreatedAtAction(
			nameof(GetById),
			new { id = result.Id },
			result);
	}
}