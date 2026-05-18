namespace CoffeeLearn.Application.SizeGroups.DTOs;

public class SizeOptionDto
{
	public int Id { get; set; }
	public int SizeGroupId { get; set; }
	public string Name { get; set; } = string.Empty;
	public int SortOrder { get; set; }
	public bool IsActive { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
}

