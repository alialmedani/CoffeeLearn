namespace CoffeeLearn.Domain.Common;

public abstract class BaseEntity
{
	public int Id { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public DateTime? UpdatedAt { get; set; }

	public bool IsDeleted { get; set; }

	public DateTime? DeletedAt { get; set; }

	public void MarkAsUpdated()
	{
		UpdatedAt = DateTime.UtcNow;
	}
	public void Restore()
	{
		IsDeleted = false;
		DeletedAt = null;
	}
	public void MarkAsDeleted(DateTime deletedAt)
	{
		IsDeleted = true;
		DeletedAt = deletedAt;
		UpdatedAt = deletedAt;
	}
}