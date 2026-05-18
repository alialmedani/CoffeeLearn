using CoffeeLearn.Domain.Common;

namespace CoffeeLearn.Domain.Entities;

public class CategorySizeOption : BaseEntity
{
	public int CategoryId { get; private set; }
	public Category Category { get; private set; } = null!;

	public string SizeName { get; private set; } = null!;
	public int SortOrder { get; private set; }
	public bool IsActive { get; private set; }

	private CategorySizeOption()
	{
	}

	public CategorySizeOption(int categoryId, string sizeName, int sortOrder = 0)
	{
		CategoryId = categoryId;
		SizeName = sizeName;
		SortOrder = sortOrder;
		IsActive = true;
	}

	public void Update(string sizeName, int sortOrder)
	{
		SizeName = sizeName;
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