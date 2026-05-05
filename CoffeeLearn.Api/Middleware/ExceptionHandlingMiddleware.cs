using FluentValidation;
using CoffeeLearn.Application.Common.Exceptions;

namespace CoffeeLearn.Api.Middleware;

public class ExceptionHandlingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionHandlingMiddleware> _logger;

	public ExceptionHandlingMiddleware(
		RequestDelegate next,
		ILogger<ExceptionHandlingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (ValidationException ex)
		{
			_logger.LogWarning(ex, "Validation error occurred.");

			var errors = ex.Errors
				.GroupBy(x => x.PropertyName)
				.ToDictionary(
					g => g.Key,
					g => g.Select(x => x.ErrorMessage).ToArray()
				);

			context.Response.StatusCode = StatusCodes.Status400BadRequest;
			context.Response.ContentType = "application/json";

			await context.Response.WriteAsJsonAsync(new
			{
				title = "Validation failed",
				status = 400,
				errors
			});
		}
		catch (NotFoundException ex)
		{
			_logger.LogWarning(ex, "Resource not found.");

			context.Response.StatusCode = StatusCodes.Status404NotFound;
			context.Response.ContentType = "application/json";

			await context.Response.WriteAsJsonAsync(new
			{
				title = "Not Found",
				status = 404,
				detail = ex.Message
			});
		}
		catch (BusinessRuleException ex)
		{
			_logger.LogWarning(ex, "Business rule violation.");

			context.Response.StatusCode = StatusCodes.Status409Conflict;
			context.Response.ContentType = "application/json";

			await context.Response.WriteAsJsonAsync(new
			{
				title = "Business Rule Violation",
				status = 409,
				detail = ex.Message
			});
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled exception occurred.");

			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			context.Response.ContentType = "application/json";

			await context.Response.WriteAsJsonAsync(new
			{
				title = "Internal Server Error",
				status = 500,
				detail = "An unexpected error occurred."
			});
		}
	}
}