namespace CoffeeLearn.Application.Orders.Common;

public abstract class OrderListQueryBase
{
	public int SkipCount { get; set; } = 0;
	public int? MaxResultCount { get; set; }

	public string? SortBy { get; set; }
	public string? SortDirection { get; set; } = "desc";
}





