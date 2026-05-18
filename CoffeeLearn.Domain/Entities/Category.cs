using CoffeeLearn.Domain.Common;

namespace CoffeeLearn.Domain.Entities;

public class Category : BaseEntity
{
	public string Name { get; private set; } = null!;
	public string? Description { get; private set; }
	public bool IsActive { get; private set; }

	public List<Product> Products { get; private set; } = new();
	public List<SizeGroup> SizeGroups { get; private set; } = new();
	private Category()
	{
	}

	public Category(string name, string? description)
	{
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