using MediatR;
using Microsoft.AspNetCore.Mvc;
using CoffeeLearn.Application.Orders.Commands;
using CoffeeLearn.Application.Orders.DTOs;

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

	[HttpPost]
	public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderCommand command)
	{
		var result = await _sender.Send(command);
		return Ok(result);
	}
}