using System.Text.Json.Serialization;
using FluentValidation;
using MediatR;
using CoffeeLearn.Application;
using CoffeeLearn.Application.Common.Behaviors;
using CoffeeLearn.Infrastructure;
using CoffeeLearn.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
	cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<AssemblyReference>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();

	app.UseSwaggerUI(c =>
	{
		c.RoutePrefix = string.Empty;
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoffeeLearn.Api v1");
	});
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();

app.Run();