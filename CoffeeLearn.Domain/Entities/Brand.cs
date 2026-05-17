using CoffeeLearn.Domain.Common;

namespace CoffeeLearn.Domain.Entities;

public class Brand : BaseEntity
{
	public string Name { get; private set; } = string.Empty;
	public string? Description { get; private set; }
	public bool IsActive { get; private set; } = true;

	public List<Product> Products { get; private set; } = new();

	private Brand()
	{
	}

	public Brand(string name, string? description)
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