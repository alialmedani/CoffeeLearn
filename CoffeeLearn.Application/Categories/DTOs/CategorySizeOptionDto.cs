namespace CoffeeLearn.Application.Categories.DTOs;

public class CategorySizeOptionDto
{
	public int Id { get; set; }
	public int CategoryId { get; set; }
	public string SizeName { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public bool IsActive { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
}