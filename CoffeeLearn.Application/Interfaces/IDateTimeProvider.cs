namespace CoffeeLearn.Application.Interfaces;

public interface IDateTimeProvider
{
	DateTime UtcNow { get; }
}