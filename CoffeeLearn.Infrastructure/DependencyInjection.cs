using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Infrastructure.Persistence;
using CoffeeLearn.Infrastructure.Services;

namespace CoffeeLearn.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<AppDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

		services.AddScoped<IApplicationDbContext>(provider =>
			provider.GetRequiredService<AppDbContext>());

		services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

		return services;
	}
}