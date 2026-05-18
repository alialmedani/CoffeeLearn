namespace CoffeeLearn.Application.Categories.DTOs;

public class CategoryDto
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public bool IsActive { get; set; }
	public bool IsDeleted { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public DateTime? DeletedAt { get; set; }
}




