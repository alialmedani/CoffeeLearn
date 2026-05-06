namespace CoffeeLearn.Application.Orders.Common;

public abstract class OrderListQueryBase
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;

	public string? SortBy { get; set; }
	public string? SortDirection { get; set; } = "desc";
}