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
	public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
	public DbSet<Order> Orders => Set<Order>();
	public DbSet<OrderItem> OrderItems => Set<OrderItem>();
	public DbSet<UploadedFile> UploadedFiles => Set<UploadedFile>();
	public DbSet<Category> Categories => Set<Category>();
	public DbSet<Brand> Brands => Set<Brand>();
	public DbSet<SizeGroup> SizeGroups => Set<SizeGroup>();
	public DbSet<SizeOption> SizeOptions => Set<SizeOption>();
	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		var entries = ChangeTracker
			.Entries<BaseEntity>()
			.Where(x =>
				x.State == EntityState.Added ||
				x.State == EntityState.Modified ||
				x.State == EntityState.Deleted)
			.ToList();

		foreach (var entry in entries)
		{
			if (entry.State == EntityState.Added)
			{
				entry.Entity.CreatedAt = _dateTimeProvider.UtcNow;
				entry.Entity.UpdatedAt = null;
				entry.Entity.IsDeleted = false;
				entry.Entity.DeletedAt = null;
			}

			if (entry.State == EntityState.Modified)
			{
				entry.Entity.UpdatedAt = _dateTimeProvider.UtcNow;
				entry.Property(x => x.CreatedAt).IsModified = false;
			}

			if (entry.State == EntityState.Deleted)
			{
				entry.State = EntityState.Modified;
				entry.Entity.MarkAsDeleted(_dateTimeProvider.UtcNow);
				entry.Property(x => x.CreatedAt).IsModified = false;
			}
		}

		return base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Category>(entity =>
		{
			entity.Property(x => x.Name)
				.HasMaxLength(200)
				.IsRequired();

			entity.Property(x => x.Description)
				.HasMaxLength(1000);

			entity.Property(x => x.IsActive)
				.IsRequired();

			entity.HasQueryFilter(x => !x.IsDeleted);
		});

		modelBuilder.Entity<Brand>(entity =>
		{
			entity.Property(x => x.Name)
				.HasMaxLength(200)
				.IsRequired();

			entity.Property(x => x.Description)
				.HasMaxLength(1000);

			entity.Property(x => x.IsActive)
				.IsRequired();

			entity.HasQueryFilter(x => !x.IsDeleted);
		});

		modelBuilder.Entity<Product>(entity =>
		{
			entity.Property(x => x.Name)
				.HasMaxLength(200)
				.IsRequired();

			entity.Property(x => x.Price)
				.HasColumnType("decimal(18,2)");

			entity.Property(x => x.Description)
				.HasMaxLength(1000);

			entity.Property(x => x.ImageUrl)
				.HasMaxLength(500);

			entity.HasOne(x => x.Category)
				.WithMany(x => x.Products)
				.HasForeignKey(x => x.CategoryId)
				.OnDelete(DeleteBehavior.SetNull);

			entity.HasOne(x => x.Brand)
				.WithMany(x => x.Products)
				.HasForeignKey(x => x.BrandId)
				.OnDelete(DeleteBehavior.SetNull);

			entity.HasQueryFilter(x => !x.IsDeleted);
		});
	 
		modelBuilder.Entity<ProductVariant>(entity =>
		{
			entity.Property(x => x.Color)
				.HasMaxLength(100)
				.IsRequired();

			entity.HasOne(x => x.SizeOption)
	.WithMany()
	.HasForeignKey(x => x.SizeOptionId)
	.OnDelete(DeleteBehavior.NoAction);

			entity.Property(x => x.Sku)
				.HasMaxLength(100);

			entity.Property(x => x.ImageUrl)
				.HasMaxLength(500);

			entity.Property(x => x.Quantity)
				.IsRequired();

			entity.Property(x => x.IsActive)
				.IsRequired();

			entity.HasOne(x => x.Product)
				.WithMany(x => x.Variants)
				.HasForeignKey(x => x.ProductId)
				.OnDelete(DeleteBehavior.Cascade);

			entity.HasQueryFilter(x => !x.IsDeleted);
		});

		modelBuilder.Entity<Order>(entity =>
		{
			entity.Property(x => x.Status)
				.IsRequired();

			entity.Property(x => x.CreatedAt)
				.IsRequired();

			entity.HasMany(x => x.Items)
				.WithOne(x => x.Order)
				.HasForeignKey(x => x.OrderId)
				.OnDelete(DeleteBehavior.Cascade);

			entity.HasQueryFilter(x => !x.IsDeleted);
		});

		modelBuilder.Entity<OrderItem>(entity =>
		{
			entity.Property(x => x.Price)
				.HasColumnType("decimal(18,2)");

			entity.HasOne(x => x.ProductVariant)
				.WithMany()
				.HasForeignKey(x => x.ProductVariantId)
				.OnDelete(DeleteBehavior.Restrict);

			entity.HasQueryFilter(x => !x.IsDeleted);
		});

		modelBuilder.Entity<UploadedFile>(entity =>
		{
			entity.Property(x => x.EntityId)
				.HasMaxLength(100)
				.IsRequired();

			entity.Property(x => x.OriginalFileName)
				.HasMaxLength(255)
				.IsRequired();

			entity.Property(x => x.StoredFileName)
				.HasMaxLength(255)
				.IsRequired();

			entity.Property(x => x.ContentType)
				.HasMaxLength(100)
				.IsRequired();

			entity.Property(x => x.FilePath)
				.HasMaxLength(500)
				.IsRequired();

			entity.Property(x => x.FileUrl)
				.HasMaxLength(500)
				.IsRequired();

			entity.Property(x => x.EntityType)
				.IsRequired();

			entity.Property(x => x.FilePlacement)
				.IsRequired();

			entity.Property(x => x.CreatedAt)
				.IsRequired();

			entity.HasQueryFilter(x => !x.IsDeleted);
		});

		base.OnModelCreating(modelBuilder);
	}
}