using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<Product> Products => Set<Product>();
	public DbSet<Order> Orders => Set<Order>();
	public DbSet<OrderItem> OrderItems => Set<OrderItem>();

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