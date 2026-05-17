using CoffeeLearn.Domain.Common;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Domain.Entities;

public class Order : BaseEntity
{
	public Guid UserId { get; set; }
	public Guid FloorId { get; set; }

	public OrderStatus Status { get; set; } = OrderStatus.Pending;

	public Guid? AcceptedByOfficeBoyId { get; set; }

	public DateTime? AcceptedAt { get; set; }
	public DateTime? CompletedAt { get; set; }
	public DateTime? CancelledAt { get; set; }

	public List<OrderItem> Items { get; set; } = new();

	public void Accept(Guid officeBoyId)
	{
		if (officeBoyId == Guid.Empty)
			throw new InvalidOperationException("Office boy id is required.");

		if (Status != OrderStatus.Pending)
			throw new InvalidOperationException("Only pending orders can be accepted.");

		Status = OrderStatus.Accepted;
		AcceptedByOfficeBoyId = officeBoyId;
		AcceptedAt = DateTime.UtcNow;

		MarkAsUpdated();
	}

	public void Complete()
	{
		if (Status != OrderStatus.Accepted)
			throw new InvalidOperationException("Only accepted orders can be completed.");

		Status = OrderStatus.Completed;
		CompletedAt = DateTime.UtcNow;

		MarkAsUpdated();
	}

	public void Cancel()
	{
		if (Status == OrderStatus.Completed)
			throw new InvalidOperationException("Completed orders cannot be cancelled.");

		if (Status == OrderStatus.Cancelled)
			throw new InvalidOperationException("Order is already cancelled.");

		Status = OrderStatus.Cancelled;
		CancelledAt = DateTime.UtcNow;

		MarkAsUpdated();
	}
}