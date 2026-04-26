using MediatR;
using Microsoft.AspNetCore.Mvc;
using CoffeeLearn.Application.Orders.Commands;
using CoffeeLearn.Application.Orders.DTOs;
using CoffeeLearn.Application.Orders.Queries;

namespace CoffeeLearn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
	private readonly ISender _sender;

	public OrdersController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet]
	public async Task<ActionResult<List<OrderDto>>> GetAll()
	{
		var result = await _sender.Send(new GetOrdersQuery());
		return Ok(result);
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<OrderDto>> GetById(int id)
	{
		var result = await _sender.Send(new GetOrderByIdQuery { Id = id });

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPost]
	public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderCommand command)
	{
		var result = await _sender.Send(command);
		return Ok(result);
	}

	[HttpPut("{id:int}/accept")]
	public async Task<ActionResult<OrderDto>> Accept(int id, [FromBody] AcceptOrderCommand command)
	{
		command.OrderId = id;

		var result = await _sender.Send(command);

		if (result is null)
			return NotFound();

		return Ok(result);
	}

	[HttpPut("{id:int}/complete")]
	public async Task<ActionResult<OrderDto>> Complete(int id)
	{
		var result = await _sender.Send(new CompleteOrderCommand { OrderId = id });

		if (result is null)
			return NotFound();

		return Ok(result);
	}
}