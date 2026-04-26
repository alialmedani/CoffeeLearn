using System.Reflection.Emit;
using CoffeeLearn.Application.Interfaces;
using CoffeeLearn.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoffeeLearn.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<Product> Products => Set<Product>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Product>(entity =>
		{
			entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
			entity.Property(x => x.Price).HasColumnType("decimal(18,2)");
		});

		base.OnModelCreating(modelBuilder);
	}
}