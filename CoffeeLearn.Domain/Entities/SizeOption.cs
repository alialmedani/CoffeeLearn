using CoffeeLearn.Domain.Common;

namespace CoffeeLearn.Domain.Entities;

public class SizeOption : BaseEntity
{
	public int SizeGroupId { get; private set; }
	public SizeGroup SizeGroup { get; private set; } = null!;

	public string Name { get; private set; } = null!;
	public int SortOrder { get; private set; }
	public bool IsActive { get; private set; }

	private SizeOption()
	{
	}

	public SizeOption(int sizeGroupId, string name, int sortOrder = 0)
	{
		SizeGroupId = sizeGroupId;
		Name = name;
		SortOrder = sortOrder;
		IsActive = true;
	}

	public void Update(string name, int sortOrder)
	{
		Name = name;
		SortOrder = sortOrder;
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