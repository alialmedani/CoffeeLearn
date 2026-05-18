namespace CoffeeLearn.Application.SizeGroups.DTOs;

public class SizeGroupDto
{
	public int Id { get; set; }
	public int CategoryId { get; set; }
	public string CategoryName { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public bool IsActive { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }

	public List<SizeOptionDto> SizeOptions { get; set; } = new();
}


