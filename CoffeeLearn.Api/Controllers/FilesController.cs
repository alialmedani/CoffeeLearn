using MediatR;
using Microsoft.AspNetCore.Mvc;
using CoffeeLearn.Api.Models;
using CoffeeLearn.Application.Files.Commands;
using CoffeeLearn.Application.Files.DTOs;
using CoffeeLearn.Application.Files.Queries;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
	private readonly ISender _sender;

	public FilesController(ISender sender)
	{
		_sender = sender;
	}

	[HttpPost("upload")]
	[Consumes("multipart/form-data")]
	public async Task<ActionResult<UploadedFileDto>> Upload([FromForm] UploadFileRequest request)
	{
		if (request.File is null || request.File.Length == 0)
			return BadRequest("File is required.");
 

		await using var stream = request.File.OpenReadStream();

		var result = await _sender.Send(new UploadFileCommand
		{
			EntityId = request.EntityId,
			EntityType = request.EntityType,
			FilePlacement = request.FilePlacement,
			ReplaceExisting = request.ReplaceExisting,
			FileStream = stream,
			OriginalFileName = request.File.FileName,
			ContentType = request.File.ContentType,
			FileSize = request.File.Length
		});

		return Ok(result);
	}

	[HttpGet]
	public async Task<ActionResult<List<UploadedFileDto>>> GetByEntity(
		[FromQuery] string entityId,
		[FromQuery] FileEntityType entityType,
		[FromQuery] FilePlacement? filePlacement)
	{
		var result = await _sender.Send(new GetFilesByEntityQuery
		{
			EntityId = entityId,
			EntityType = entityType,
			FilePlacement = filePlacement
		});

		return Ok(result);
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var result = await _sender.Send(new DeleteFileCommand(id));

		if (!result)
			return NotFound();

		return NoContent();
	}
	[HttpGet("{id:int}")]
	public async Task<ActionResult<UploadedFileDto>> GetById(int id)
	{
		var result = await _sender.Send(new GetFileByIdQuery(id));

		if (result is null)
			return NotFound();

		return Ok(result);
	}
}