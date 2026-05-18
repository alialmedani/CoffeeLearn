using CoffeeLearn.Domain.Common;

namespace CoffeeLearn.Domain.Entities;

public class SizeGroup : BaseEntity
{
	public int CategoryId { get; private set; }
	public Category Category { get; private set; } = null!;

	public string Name { get; private set; } = null!;
	public string? Description { get; private set; }
	public bool IsActive { get; private set; }

	public List<SizeOption> SizeOptions { get; private set; } = new();

	private SizeGroup()
	{
	}

	public SizeGroup(int categoryId, string name, string? description)
	{
		CategoryId = categoryId;
		Name = name;
		Description = description;
		IsActive = true;
	}

	public void Update(string name, string? description)
	{
		Name = name;
		Description = description;
		MarkAsUpdated();
	}

	public void Activate()
	{
		IsActive = true;
		MarkAsUpdated();
	}

	public void Deactivate()
	{
		IsActive = false;
		MarkAsUpdated();
	}
}