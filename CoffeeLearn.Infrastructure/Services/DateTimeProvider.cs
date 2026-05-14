using CoffeeLearn.Application.Interfaces;

namespace CoffeeLearn.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
	public DateTime UtcNow => DateTime.UtcNow;
}