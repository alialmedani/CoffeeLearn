using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Domain.Common;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
	private readonly IDateTimeProvider _dateTimeProvider;

	public AppDbContext(
		DbContextOptions<AppDbContext> options,
		IDateTimeProvider dateTimeProvider) : base(options)
	{
		_dateTimeProvider = dateTimeProvider;
	}

	public DbSet<Product> Products => Set<Product>();
	public DbSet<Order> Orders => Set<Order>();
	public DbSet<OrderItem> OrderItems => Set<OrderItem>();

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		var entries = ChangeTracker
			.Entries<BaseEntity>()
			.Where(x => x.State == EntityState.Added || x.State == EntityState.Modified)
			.ToList();

		foreach (var entry in entries)
		{
			if (entry.State == EntityState.Added)
			{
				entry.Entity.CreatedAt = _dateTimeProvider.UtcNow;
				entry.Entity.UpdatedAt = null;
			}

			if (entry.State == EntityState.Modified)
			{
				entry.Entity.UpdatedAt = _dateTimeProvider.UtcNow;

				entry.Property(x => x.CreatedAt).IsModified = false;
			}
		}

		return base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Product>(entity =>
		{
			entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
			entity.Property(x => x.Price).HasColumnType("decimal(18,2)");
		});

		modelBuilder.Entity<Order>(entity =>
		{
			entity.Property(x => x.Status).IsRequired();
			entity.Property(x => x.CreatedAt).IsRequired();

			entity.HasMany(x => x.Items)
				.WithOne(x => x.Order)
				.HasForeignKey(x => x.OrderId)
				.OnDelete(DeleteBehavior.Cascade);
		});

		modelBuilder.Entity<OrderItem>(entity =>
		{
			entity.Property(x => x.Price).HasColumnType("decimal(18,2)");
		});

		base.OnModelCreating(modelBuilder);
	}
}