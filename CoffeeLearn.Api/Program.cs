using FluentValidation;
using MediatR;
using CoffeeLearn.Application;
using CoffeeLearn.Application.Common.Behaviors;
using CoffeeLearn.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
