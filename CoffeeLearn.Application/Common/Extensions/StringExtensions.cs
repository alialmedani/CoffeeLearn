namespace CoffeeLearn.Application.Common.Extensions;

public static class StringExtensions
{
	public static string NormalizeText(this string? value)
	{
		return value?.Trim().ToLower() ?? string.Empty;
	}
}

