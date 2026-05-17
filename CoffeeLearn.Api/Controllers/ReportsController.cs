using CoffeeLearn.Application.Reports.DTOs;
using CoffeeLearn.Application.Reports.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeLearn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
	private readonly ISender _sender;

	public ReportsController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet("low-stock-variants")]
	public async Task<ActionResult<List<LowStockVariantDto>>> GetLowStockVariants(
		[FromQuery] GetLowStockVariantsQuery query)
	{
		var result = await _sender.Send(query);
		return Ok(result);
	}
}