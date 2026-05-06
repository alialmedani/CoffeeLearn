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

	[HttpGet("my")]
	public async Task<ActionResult<List<OrderDto>>> GetMyOrders([FromQuery] Guid userId)
	{
		var result = await _sender.Send(new GetMyOrdersQuery { UserId = userId });
		return Ok(result);
	}

	[HttpGet("by-floor")]
	public async Task<ActionResult<List<OrderDto>>> GetByFloor([FromQuery] Guid floorId)
	{
		var result = await _sender.Send(new GetOrdersByFloorQuery { FloorId = floorId });
		return Ok(result);
	}

	[HttpGet("pending-by-floor")]
	public async Task<ActionResult<List<OrderDto>>> GetPendingByFloor([FromQuery] Guid floorId)
	{
		var result = await _sender.Send(new GetPendingOrdersByFloorQuery { FloorId = floorId });
		return Ok(result);
	}

	[HttpGet("accepted-by-officeboy")]
	public async Task<ActionResult<List<OrderDto>>> GetAcceptedByOfficeBoy([FromQuery] Guid officeBoyId)
	{
		var result = await _sender.Send(new GetAcceptedOrdersByOfficeBoyQuery { OfficeBoyId = officeBoyId });
		return Ok(result);
	}

	[HttpGet("completed-by-floor")]
	public async Task<ActionResult<List<OrderDto>>> GetCompletedByFloor([FromQuery] Guid floorId)
	{
		var result = await _sender.Send(new GetCompletedOrdersByFloorQuery { FloorId = floorId });
		return Ok(result);
	}

	[HttpPost]
	public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderCommand command)
	{
		var result = await _sender.Send(command);

		return CreatedAtAction(
			nameof(GetById),
			new { id = result.Id },
			result);
	}

	[HttpPut("{id:int}/accept")]
	public async Task<ActionResult<OrderDto>> Accept(int id, [FromBody] AcceptOrderCommand command)
	{
		command.OrderId = id;

		var result = await _sender.Send(command);
		return Ok(result);
	}

	[HttpPut("{id:int}/complete")]
	public async Task<ActionResult<OrderDto>> Complete(int id)
	{
		var result = await _sender.Send(new CompleteOrderCommand { OrderId = id });
		return Ok(result);
	}

	[HttpPut("{id:int}/cancel")]
	public async Task<ActionResult<OrderDto>> Cancel(int id)
	{
		var result = await _sender.Send(new CancelOrderCommand { OrderId = id });
		return Ok(result);
	}
}